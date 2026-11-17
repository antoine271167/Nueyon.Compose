const fs = require('node:fs');
const path = require('node:path');
const os = require('node:os');
const { createHash } = require('node:crypto');
const { execFileSync } = require('node:child_process');
const assert = require('node:assert/strict');

const repo = path.resolve(__dirname, '../../..');
const relativeExperiment = 'PromptExperiments/Synthesis/experiment-2-topic-neutral-2026-11-16';
const experiment = path.join(repo, relativeExperiment);
const read = file => JSON.parse(fs.readFileSync(file, 'utf8').replace(/^\uFEFF/, ''));
const old = file => read(path.join(experiment, file));
const hash = file => createHash('sha256').update(fs.readFileSync(file)).digest('hex');
const frozen = old('freeze.json').files;
const artifacts = old('artifact-manifest.json').files;
for (const [base, manifest] of [[repo, frozen], [experiment, artifacts]]) {
  for (const [file, expected] of Object.entries(manifest)) {
    assert.equal(hash(path.join(base, file)), expected, file);
  }
}

const original = old('assessments.json');
const decisions = read(path.join(__dirname, 'reassessment.json'));
assert.deepEqual(decisions.dimensionOrder, original.dimensions);
const reviews = new Map(decisions.reviews.map(row => [row.id, row]));
assert.equal(reviews.size, 5);
const storedRequests = new Map(old('verification-requests.json').map(row => [row.id, row]));
const schedule = old('plan.json').schedule;
const cases = [...new Set(schedule.map(row => row.case))];
const rows = original.runs.map(row => {
  const id = row.id + '-Synthesis';
  const review = reviews.get(id);
  const dir = 'calls/' + id;
  const caseDir = 'cases/' + row.case;
  const raw = old(dir + '/response-raw.json').choices[0];
  const result = old(dir + '/result.json');
  const input = old(dir + '/input.json');
  const content = fs.readFileSync(path.join(experiment, dir, 'content.md'), 'utf8');
  const projection = fs.readFileSync(path.join(experiment, caseDir, 'projection.md'), 'utf8');
  const storedProjection = fs.readFileSync(path.join(experiment, caseDir, 'stored-projection.md'), 'utf8');
  assert.deepEqual(input, old(caseDir + '/input.json'), id);
  for (const [key, value] of Object.entries(old(caseDir + '/idea.json'))) {
    assert.equal(input.selectedIdea[key[0].toLowerCase() + key.slice(1)], value, id);
  }
  assert.equal(input.research.content, projection, id);
  assert.equal(storedProjection, projection, id);

  assert.equal(content.trimEnd(), result.content.trimEnd(), id);
  const requestCheck = storedRequests.get(id);
  const valid = raw.finish_reason === 'stop' && requestCheck &&
    Object.entries(requestCheck).every(([key, value]) => key === 'id' || value === true);
  const scores = review?.scores ?? row.scores;
  return {
    id, case: row.case, arm: row.arm, originalScores: row.scores, scores,
    originalPass: row.pass, pass: scores.every(score => score === 2), valid: Boolean(valid),
    reassessed: Boolean(review),
    actorContamination: review?.actorContamination ??
      row.findings.some(finding => finding.kind === 'actorcontaminatie'),
    materialMisleadingClaim: review?.materialMisleadingClaim ?? scores.includes(0),
    essentialOmissions: review?.essentialOmissions ?? []
  };
});
assert.equal(rows.length, 24);
assert.equal(rows.filter(row => row.reassessed).length, 5);
assert.deepEqual(rows.map(row => row.id).sort(), schedule.map(row => row.id + '-Synthesis').sort());

const scratch = fs.mkdtempSync(path.join(os.tmpdir(), 'nueyon-synthesis-exp3-replay-'));
for (const file of Object.keys(frozen)) {
  const target = path.join(scratch, file);
  fs.mkdirSync(path.dirname(target), { recursive: true });
  fs.copyFileSync(path.join(repo, file), target);
}
const replayDir = path.join(scratch, relativeExperiment);
fs.copyFileSync(path.join(experiment, 'freeze.json'), path.join(replayDir, 'freeze.json'));
fs.cpSync(path.join(experiment, 'calls'), path.join(replayDir, 'calls'), { recursive: true });
const runner = path.join(replayDir, 'runner/bin/Debug/net10.0/Runner.dll');
const preflight = execFileSync('dotnet', [runner, 'preflight'], { cwd: scratch, encoding: 'utf8' }).trim();
assert.deepEqual(read(path.join(replayDir, 'verification-preflight.json')), old('verification-preflight.json'));
const replay = execFileSync('dotnet', [runner, 'replay'], { cwd: scratch, encoding: 'utf8' }).trim();
const replayRows = read(path.join(replayDir, 'verification-offline-replay.json'));
assert.deepEqual(replayRows, old('verification-offline-replay.json'));
assert.deepEqual(replayRows.map(row => row.id).sort(), rows.map(row => row.id).sort());
assert.ok(replayRows.every(row => row.parsedAndExact));

const count = (list, predicate) => list.filter(predicate).length;
const byArm = Object.fromEntries(['A', 'B'].map(arm => [arm, rows.filter(row => row.arm === arm)]));
const passCount = list => count(list, row => row.pass);
const perCase = list => Object.fromEntries(cases.map(name => [name, passCount(list.filter(row => row.case === name))]));
const armSummary = list => ({
  total: list.length, valid: count(list, row => row.valid), pass: passCount(list),
  cases: perCase(list), actorContaminations: count(list, row => row.actorContamination),
  materialMisleadingClaims: count(list, row => row.materialMisleadingClaim)
});
const A = armSummary(byArm.A);
const B = armSummary(byArm.B);
const pairs = byArm.A.map(a => {
  const b = byArm.B.find(row => row.id === a.id.replace('-A-Synthesis', '-B-Synthesis'));
  assert.ok(b, a.id);
  return { id: a.id.replace('-A-Synthesis', ''), delta: b.scores.map((score, i) => score - a.scores[i]) };
});
const dimensions = original.dimensions.map((dimension, index) => ({
  dimension, lower: count(pairs, pair => pair.delta[index] < 0),
  higher: count(pairs, pair => pair.delta[index] > 0)
}));
const repeatedOmissions = cases.flatMap(name => {
  const omissions = byArm.B.filter(row => row.case === name).flatMap(row => row.essentialOmissions);
  return [...new Set(omissions)].filter(item => count(omissions, entry => entry === item) > 1)
    .map(item => ({ case: name, omission: item }));
});
assert.ok(byArm.B.filter(row => !row.reassessed).every(row => row.pass),
  'Retained B assessments must all be passes to infer no essential omissions from their scores.');
const gates = {
  twelveValidB: B.valid === 12,
  noMaterialActorContaminationB: B.actorContaminations === 0,
  noMaterialFabricatedOrMisleadingClaimsB: B.materialMisleadingClaims === 0,
  minimum11of12: B.pass >= 11,
  atLeast2of3EveryCase: cases.every(name => B.cases[name] >= 2),
  noRepeatedEssentialOmissionWithinBCase: repeatedOmissions.length === 0,
  noLowerTotal: B.pass >= A.pass,
  noLowerCase: cases.every(name => B.cases[name] >= A.cases[name]),
  noUncompensatedRepeatedDimensionLoss: dimensions.every(row => row.lower < 2 || row.higher > 0),
  brandCorrectionObserved: A.actorContaminations > 0 && B.actorContaminations === 0
};
const result = {
  verdict: Object.values(gates).every(Boolean) ? 'GO' : 'NO-GO',
  A, B, gates, repeatedOmissions, dimensions, pairs, rows,
  verification: {
    repoHead: execFileSync('git', ['rev-parse', 'HEAD'], { cwd: repo, encoding: 'utf8' }).trim(),
    frozenFiles: Object.keys(frozen).length,
    artifactFiles: Object.keys(artifacts).length,
    reassessmentSha256: hash(path.join(__dirname, 'reassessment.json')),
    protocolSha256: hash(path.join(experiment, 'protocol.md')),
    preflight, replay, exactReplays: replayRows.length
  }
};
fs.writeFileSync(path.join(__dirname, 'gate-replay.json'), JSON.stringify(result, null, 2) + '\n');
console.log(JSON.stringify({ verdict: result.verdict, A, B, gates, dimensions, verification: result.verification, scratch }, null, 2));

