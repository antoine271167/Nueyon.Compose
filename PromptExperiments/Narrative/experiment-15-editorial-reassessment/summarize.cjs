const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { rows, read, csv, hash } = require('./inventory.cjs');
const weights = [25, 25, 25, 15, 10];
const freeze = {
  'protocol.md': '71b1f00260add2b04825620d4f013e98f84cd4512a16f3bd88b0c1b2f2362fc5',
  'review.txt': '8a87220c077a6ad501981f20ffada1d8d30be23d5bf09d7ed14469badc1cd1a8',
  'second-review.md': '02d5f682b9f59603a766a5bf61538cf6270586680aecf5b392e48d59681c1492',
};
for (const [file, digest] of Object.entries(freeze)) {
  const content = fs.readFileSync(path.join(__dirname, file), 'utf8').replaceAll('\r\n', '\n');
  assert.equal(hash(content), digest, file);
}
const scores = new Map();
for (const line of fs.readFileSync(path.join(__dirname, 'review.txt'), 'utf8').trim().split(/\r?\n/).slice(1)) {
  const match = line.match(/^([\da-f]{10}) ([1-5]{5}) ([012]) (\S+) (.+)$/);
  assert.ok(match, line);
  const [, id, digits, fidelity, issue, rationale] = match;
  assert.ok(!scores.has(id), id);
  const [direction, development, usability, resolution, evidence] = [...digits].map(Number);
  const total = [...digits].reduce((sum, digit, i) => sum + Number(digit) * weights[i], 0);
  const editorialPass = total >= 350 && Math.min(direction, development, usability) >= 3;
  assert.equal(issue === '-', fidelity === '0', id);
  scores.set(id, { id, direction, development, usability, resolution, evidence, total,
    weighted: (total / 100).toFixed(2), fidelity: 'F' + fidelity, issue, rationale,
    editorialPass, accepted: editorialPass && fidelity !== '2' });
}
const eligible = rows.filter(row => row.status === 'eligible');
assert.equal(eligible.length, scores.size);
assert.ok(eligible.every(row => scores.has(row.id) && !row.missingReferences));
const previous = new Map();
for (const folder of new Set(rows.map(row => row.output.split('/runs/')[0]))) {
  const [header, ...lines] = read(folder + '/scores.csv').toString().trim().split(/\r?\n/);
  const index = header.replaceAll('"', '').split(',').findIndex(field => ['complete', 'full'].includes(field));
  assert.ok(index > 0);
  for (const line of lines) {
    const fields = line.replaceAll('"', '').split(',');
    previous.set(folder + '/' + fields[0], Number(fields[index]));
  }
}
const joined = eligible.map(row => {
  const priorPass = previous.get(row.output.split('/runs/')[0] + '/' + row.run);
  assert.ok(priorPass === 0 || priorPass === 1, row.run);
  return { ...row, ...scores.get(row.id), priorPass };
});
function summarize(cohort) {
  const distribution = {};
  for (const row of [...cohort].sort((a, b) => a.total - b.total)) {
    distribution[row.weighted] = (distribution[row.weighted] ?? 0) + 1;
  }
  return {
    n: cohort.length,
    mean: Number((cohort.reduce((sum, row) => sum + row.total, 0) / cohort.length / 100).toFixed(3)),
    distribution,
    editorialPass: cohort.filter(row => row.editorialPass).length,
    accepted: cohort.filter(row => row.accepted).length,
    fidelity: Object.fromEntries(['F0', 'F1', 'F2'].map(f => [f, cohort.filter(row => row.fidelity === f).length])),
    priorPass: cohort.filter(row => row.priorPass).length,
    newlyAccepted: cohort.filter(row => !row.priorPass && row.accepted).length,
    priorPassNowRejected: cohort.filter(row => row.priorPass && !row.accepted).length,
  };
}
const production = joined.filter(row => row.variant === 'A');
const prompts = {};
for (const part of ['system', 'user']) {
  const file = `Narrative.${part}.prompt.md`;
  const currentHash = hash(read(`src/Nueyon.Compose.Application/Prompts/Narrative/${file}`));
  const matches = [...new Set(production.map(row => row.output.split('/runs/')[0]))]
    .map(folder => hash(read(`${folder}/A/Narrative/${file}`)));
  assert.ok(matches.every(digest => digest === currentHash), file);
  prompts[file] = currentHash;
}
const stats = {
  evidenceRevision: '14cd5b70c4ac84e54956cbd4397534893a1c437d',
  prompts,
  overall: summarize(joined),
  production: summarize(production),
  productionBySubject: Object.fromEntries([...new Set(production.map(row => row.subject))].sort()
    .map(subject => [subject, summarize(production.filter(row => row.subject === subject))])),
  arms: Object.fromEntries(['13A', '13B', '14A', '14B'].map(arm => [arm,
    summarize(joined.filter(row => row.experiment + row.variant === arm))])),
};
if (process.argv.includes('--write')) {
  for (const [file, content] of [
    ['manifest.csv', csv(rows)],
    ['scores.csv', csv(joined.map(({ id, experiment, variant, subject, run, priorPass }) =>
      ({ ...scores.get(id), experiment, variant, subject, run, priorPass })))],
  ]) fs.writeFileSync(path.join(__dirname, file), content);
}
console.log(JSON.stringify(stats, null, 2));
