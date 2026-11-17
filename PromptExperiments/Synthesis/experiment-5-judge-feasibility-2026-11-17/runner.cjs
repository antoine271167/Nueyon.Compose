const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const crypto = require('node:crypto');
const { execFileSync } = require('node:child_process');
process.chdir(__dirname);

const read = file => fs.readFileSync(file, 'utf8').replace(/^\uFEFF/, '');
const json = file => JSON.parse(read(file));
const save = (file, data) => fs.writeFileSync(file, JSON.stringify(data, null, 2) + '\n', { flag: 'wx' });
const hash = data => crypto.createHash('sha256').update(data).digest('hex');
const digest = file => hash(fs.readFileSync(file));
const getExperiment = number => path.join('..', fs.readdirSync('..').find(name => name.startsWith('experiment-' + number + '-')));
const model = 'gpt-6-luna';
const maxCompletion = 2000;
const endpoint = 'https://api.openai.com/v1/chat/completions';
const blockerFields = ['field', 'problem', 'outputQuote', 'evidenceLocation', 'evidenceQuote', 'correction'];
const schema = {
  type: 'object', additionalProperties: false, required: ['verdict', 'blockers'],
  properties: {
    verdict: { type: 'string', enum: ['PASS', 'FAIL'] },
    blockers: { type: 'array', items: {
      type: 'object', additionalProperties: false, required: blockerFields,
      properties: Object.fromEntries(blockerFields.map(field => [
        field, field === 'evidenceLocation'
          ? { type: 'string', enum: ['source', 'projection', 'selectedIdea'] }
          : { type: 'string' }
      ]))
    } }
  }
};

const historicalTargets = {
  'webb-r1-A': ['folding_reason', 'Size made folding for launch necessary; stray Nuëyon brand wording is not misattribution.'],
  'webb-r3-B': ['folding_reason', 'Size made folding for launch necessary; exact dimensions optional.'],
  'digital-r2-A': ['actor', 'Nuëyon’s account misattributes the OPG practice report; deadline-versus-duration also noted as precision.'],
  'wolf-r3-A': ['focus', 'Missing concrete pack cooperation and scavenger relationship; population inventory optional.'],
  'wolf-r1-B': ['species', 'Eland/elandkadaver substitutes moose for Yellowstone elk; bear detail and exact percentages optional.']
};
const primaryTargets = {
  'wolf-r1-A': 'species', 'wolf-r1-C': 'species', 'webb-r2-C': 'folding_reason',
  'webb-r1-C': 'timeline', 'digital-r1-C': 'timeline', 'digital-r2-A': 'timeline'
};

function freeze() {
  assert(!fs.existsSync('freeze.json'));
  const exp4 = getExperiment(4);
  const exp2 = getExperiment(2);
  const scores = json(path.join(exp4, 'scores.json'));
  const upstream = {};
  function track(file) {
    upstream[file] = digest(file);
    return file;
  }
  const mappings = Object.keys(scores).map(run => [4, exp4, run])
    .concat(Object.keys(historicalTargets).map(run => [2, exp2, run]));
  const sourceSet = ['source.md', 'idea.json', 'projection.md', 'research-result.json', 'input.json'];
  for (const file of [
    path.join(exp4, 'scores.json'), path.join(exp4, 'protocol.md'),
    path.join(exp2, 'protocol.md'), path.join(getExperiment(3), 'reassessment.json')
  ]) track(file);

  const system = read('judge.md');
  const prepared = mappings.map(([experiment, root, run]) => {
    const caseName = run.split('-')[0];
    const fixture = Object.fromEntries(sourceSet.map(file => [
      file, track(path.join(root, 'cases', caseName, file))
    ]));
    const call = file => track(path.join(root, 'calls', run + '-Synthesis', file));
    const synthesis = json(call('result.json')).content;
    assert.equal(read(call('content.md')).trim(), synthesis.trim(), run);
    const priorRequest = json(call('request.json')).messages.at(-1).content;
    const selectedIdea = json(fixture['idea.json']);
    const projection = read(fixture['projection.md']);
    assert(priorRequest.includes(projection.trim()), 'Projection mismatch ' + run);
    for (const value of Object.values(selectedIdea)) assert(priorRequest.includes(value), 'Idea mismatch ' + run);
    const input = { selectedIdea, source: read(fixture['source.md']), projection, synthesis };
    const request = {
      messages: [{ role: 'system', content: system }, { role: 'user', content: JSON.stringify(input) }],
      model, max_completion_tokens: maxCompletion,
      response_format: { type: 'json_schema', json_schema: { name: 'synthesis_review', strict: true, schema } }
    };
    const score = experiment === 4 ? scores[run] : null;
    const target = score ? primaryTargets[run] ?? null : historicalTargets[run][0];
    assert(!score || Boolean(target) === score.ratings.some(rating => rating < 2), run);
    const legitimate = [];
    if (caseName === 'lowell') legitimate.push('Early output/worker contrast is optional given the reuse and downturn focus.');
    if (caseName === 'webb') legitimate.push('Exact dimensions and an inventory of deployment hardware are optional.');
    if (caseName === 'wolf') legitimate.push('Bear detail and exact percentages are optional when the central relationships survive.');
    if (run === 'digital-r2-A' && experiment === 4) legitimate.push('Detailed early KPI figures are optional in this handoff.');
    return {
      order: hash('synthesis-judge-5|' + experiment + '|' + run), request,
      label: { experiment, run, expected: target ? 'FAIL' : 'PASS', target, score,
        note: score?.note ?? historicalTargets[run][1], legitimate }
    };
  }).sort((left, right) => left.order.localeCompare(right.order));
  assert.equal(prepared.length, 29);
  const sizes = prepared.map(({ request }) => Buffer.byteLength(JSON.stringify(request)));
  const inputBound = sizes.reduce((sum, size) => sum + size + 256, 0);
  assert(Math.max(...sizes) <= 26000 && inputBound <= 720000);
  const ids = prepared.map((_, index) => String(index + 1).padStart(2, '0'));
  fs.mkdirSync('calls');
  prepared.forEach(({ request }, index) => {
    const dir = 'calls/' + ids[index];
    fs.mkdirSync(dir);
    fs.writeFileSync(dir + '/request.json', JSON.stringify(request), { flag: 'wx' });
  });
  save('gold.json', Object.fromEntries(prepared.map(({ label }, index) => [ids[index], label])));
  const files = ['runner.cjs', 'judge.md', 'protocol.md', 'gold.json',
    ...ids.map(id => 'calls/' + id + '/request.json')];
  save('freeze.json', {
    at: new Date().toISOString(), head: execFileSync('git', ['rev-parse', 'HEAD'], { encoding: 'utf8' }).trim(),
    endpoint, ids, inputBound, completionBound: maxCompletion * ids.length, requestBytes: sizes, upstream,
    sha256: Object.fromEntries(files.map(file => [file, digest(file)]))
  });
}

function verifyFreeze() {
  const frozen = json('freeze.json');
  for (const [file, expected] of Object.entries(frozen.sha256)) assert.equal(digest(file), expected, file);
  return frozen;
}

async function live() {
  const frozen = verifyFreeze();
  for (const [file, expected] of Object.entries(frozen.upstream)) assert.equal(digest(file), expected, file);
  const secretsPath = path.join(process.env.APPDATA, 'Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json');
  const secrets = process.env.OPENAI_API_KEY ? {} : json(secretsPath);
  const apiKey = process.env.OPENAI_API_KEY || secrets['OpenAI:ApiKey'] || secrets.OpenAI?.ApiKey;
  if (!apiKey) throw Error('Missing OpenAI credential');
  fs.writeFileSync('run.lock', new Date().toISOString(), { flag: 'wx' });
  async function judge(id) {
    const dir = 'calls/' + id;
    save(dir + '/attempt.json', { started: new Date().toISOString() });
    try {
      const response = await fetch(endpoint, {
        method: 'POST', redirect: 'error', signal: AbortSignal.timeout(180000),
        headers: { 'Content-Type': 'application/json', Authorization: 'Bearer ' + apiKey },
        body: fs.readFileSync(dir + '/request.json')
      });
      fs.writeFileSync(dir + '/response-raw.json', Buffer.from(await response.arrayBuffer()), { flag: 'wx' });
      save(dir + '/http.json', { status: response.status, at: new Date().toISOString() });
      console.log(id, response.status);
    } catch (error) {
      save(dir + '/http.json', { error: error.name, at: new Date().toISOString() });
      console.log(id, error.name);
    }
  }
  let next = 0;
  await Promise.all(Array.from({ length: 5 }, async () => {
    while (next < frozen.ids.length) await judge(frozen.ids[next++]);
  }));
}

function replay() {
  const frozen = verifyFreeze();
  const labels = json('gold.json');
  const matrix = { primary: { TP: 0, TN: 0, FP: 0, FN: 0, invalid: 0 },
    historical: { TP: 0, TN: 0, FP: 0, FN: 0, invalid: 0 } };
  const totals = { prompt: 0, completion: 0, total: 0, cached: 0, reasoning: 0 };
  const rows = frozen.ids.map(id => {
    const dir = 'calls/' + id;
    const label = labels[id];
    const count = matrix[label.experiment === 4 ? 'primary' : 'historical'];
    const row = { id, ...label };
    try {
      const request = json(dir + '/request.json');
      const input = JSON.parse(request.messages.at(-1).content);
      const response = json(dir + '/response-raw.json');
      if (response.usage) {
        row.usage = response.usage;
        totals.prompt += response.usage.prompt_tokens;
        totals.completion += response.usage.completion_tokens;
        totals.total += response.usage.total_tokens;
        totals.cached += response.usage.prompt_tokens_details?.cached_tokens ?? 0;
        totals.reasoning += response.usage.completion_tokens_details?.reasoning_tokens ?? 0;
      }
      assert.equal(json(dir + '/http.json').status, 200);
      assert.equal(response.model, model);
      assert.equal(response.choices[0].finish_reason, 'stop');
      const result = JSON.parse(response.choices[0].message.content);
      assert.deepEqual(Object.keys(result).sort(), ['blockers', 'verdict']);
      assert(['PASS', 'FAIL'].includes(result.verdict));
      assert(Array.isArray(result.blockers));
      assert(result.verdict === 'PASS' ? result.blockers.length === 0 :
        result.blockers.length >= 1 && result.blockers.length <= 3);
      row.blockers = result.blockers.map(blocker => {
        assert.deepEqual(Object.keys(blocker).sort(), [...blockerFields].sort());
        assert(blockerFields.every(field => typeof blocker[field] === 'string' && blocker[field].trim()));
        assert(['source', 'projection', 'selectedIdea'].includes(blocker.evidenceLocation));
        const evidence = input[blocker.evidenceLocation];
        return { ...blocker, exactOutput: input.synthesis.includes(blocker.outputQuote),
          exactEvidence: (typeof evidence === 'string' ? evidence : Object.values(evidence).join('\n')).includes(blocker.evidenceQuote) };
      });
      row.verdict = result.verdict;
      row.class = (result.verdict === label.expected ? 'T' : 'F') + (result.verdict === 'FAIL' ? 'P' : 'N');
    } catch (error) {
      row.class = 'invalid';
      row.error = error.message;
    }
    count[row.class]++;
    return row;
  });
  console.log(JSON.stringify({ matrix, totals, rows }, null, 2));
}

async function main() {
  switch (process.argv[2]) {
    case 'freeze': freeze(); break;
    case 'live': await live(); break;
    case 'replay': replay(); break;
    default: throw Error('Use freeze, live or replay');
  }
}
main().catch(error => { console.error(error.message); process.exitCode = 1; });
