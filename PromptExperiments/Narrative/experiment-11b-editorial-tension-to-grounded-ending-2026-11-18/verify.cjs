const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const assert = require('node:assert/strict');
const old = path.join(__dirname, '..', 'experiment-11-editorial-tension-to-grounded-ending-2026-11-18');
const config = require(path.join(old, 'config.json'));
const invalid = config.schedule[15];
const originalIds = new Set(config.schedule.slice(0, 15));
function resolve(file) {
  if (file.startsWith('calls/')) {
    const [, id, name] = file.split('/');
    const folder = originalIds.has(id) ? old : __dirname;
    return path.join(folder, 'calls', id === invalid ? id + '-replacement' : id, name);
  }
  return path.join(file === 'replay.txt' || file.startsWith('scores/') ? __dirname : old, file);
}
const read = file => fs.readFileSync(resolve(file), 'utf8');
const json = file => JSON.parse(read(file));
const exists = file => fs.existsSync(resolve(file));
const call = (id, file) => json('calls/' + id + '/' + file + '.json');
const reviewId = id => crypto.createHash('sha256').update((originalIds.has(id) ? 'exp11/review/' : 'exp11b/review/') + id).digest('hex').slice(0, 10);
const fixtureOf = id => id.replace(/-c[12]-[ABC]$/, '');
const complete = config.schedule.filter(id => exists('calls/' + id + '/result.json'));
if (process.argv[2] === 'review') {
  for (const id of complete.filter(id => !originalIds.has(id) && id.startsWith(process.argv[3] + '-')).sort((a,b) => reviewId(a).localeCompare(reviewId(b)))) {
    console.log('\n## ' + reviewId(id) + ' · ' + fixtureOf(id) + '\n');
    console.log(read('calls/' + id + '/content.md'));
  }
  return;
}
const topics = ['digital','webb','wolf','lowell'];
const marks = Object.assign({}, ...topics.filter(t => exists('scores/' + t + '.json')).map(t => json('scores/' + t + '.json')));
Object.assign(marks, ...['digital','webb','wolf','lowell'].map(topic =>
  JSON.parse(fs.readFileSync(path.join(old, 'scores', topic + '.json'), 'utf8'))));
const archive = path.join(__dirname, 'archive', invalid + '-original');
const original = path.join(old, 'calls', invalid);
for (const file of fs.readdirSync(original)) {
  assert.deepEqual(fs.readFileSync(path.join(archive, file)), fs.readFileSync(path.join(original, file)));
}
const invalidResponse = JSON.parse(fs.readFileSync(path.join(archive, 'response-raw.json')));
assert.equal(JSON.parse(fs.readFileSync(path.join(archive, 'http.json'))).status, 200);
assert.equal(invalidResponse.model, config.model);
assert.equal(invalidResponse.choices[0].finish_reason, 'stop');
assert.deepEqual(JSON.parse(invalidResponse.choices[0].message.content), {});
if (complete.includes(invalid)) {
  for (const name of ['input.json','request.json']) {
    assert.deepEqual(fs.readFileSync(resolve('calls/' + invalid + '/' + name)), fs.readFileSync(path.join(archive, name)));
  }
}
assert.deepEqual(complete, config.schedule.slice(0, complete.length));
const responseIds = new Set([invalidResponse.id]);
const rates = config.ratesPerMillionUsd;
const stock = read('prompts/Compose/Compose.linkedin.user.prompt.md');
const edited = read('variant/Compose/Compose.linkedin.user.prompt.md');
const stockLine = stock.split(/\r?\n/).find(line => line.startsWith('- Leave the reader with'));
const editedLine = edited.split(/\r?\n/).find(line => line.startsWith('- Use a concrete detail developed'));
assert.equal(edited.replace(editedLine, stockLine), stock);
for (const [file, hash] of Object.entries(json('freeze.json'))) {
  assert.equal(crypto.createHash('sha256').update(fs.readFileSync(resolve(file))).digest('hex').toUpperCase(), hash, file);
}
assert.equal(config.schedule.length, 48);
assert.equal(new Set(config.schedule).size, 48);
for (const stratum of ['safe','problematic']) for (const arm of 'ABC') for (let position=0; position<3; position++) {
  const n = config.schedule.filter((id, i) => id.endsWith('-' + arm) && i % 3 === position &&
    config.fixtures[fixtureOf(id)] === stratum).length;
  assert.ok(n === 2 || n === 3);
}
const verificationOnly = process.argv[2] === 'requests';
const rows = complete.map(id => {
  const fixture = fixtureOf(id);
  const topic = fixture.split('-')[0];
  const arm = id.at(-1);
    assert.equal(call(id, 'http').status, 200);
  assert.equal(call(id, 'attempt').run, id);
  const request = call(id, 'request');
  assert.deepEqual(Object.keys(request).sort(), ['messages', 'model', 'response_format']);
  assert.equal(request.model, config.model);
  assert.deepEqual(request.messages.map(message => message.role), ['system', 'user']);
  if (id.includes('-c2-')) {
    const first = id.replace('-c2-', '-c1-');
    assert.deepEqual(request, call(first, 'request'), id + ' repeat request');
  }
  const response = call(id, 'response-raw');
  assert.equal(response.model, config.model);
  assert.equal(response.choices[0].finish_reason, 'stop');
  assert.ok(response.id && !responseIds.has(response.id));
  responseIds.add(response.id);
  const {prompt_tokens: input, completion_tokens: output, total_tokens: total} = response.usage;
  const cached = response.usage.prompt_tokens_details?.cached_tokens ?? 0;
  assert.equal(total, input + output);
  const usd = (rates.input * (input - cached) + rates.cached * cached + rates.output * output) / 1e6;
  const noCacheUsd = (rates.input * input + rates.output * output) / 1e6;
  const mark = marks[reviewId(id)];
  if (!verificationOnly) {
    assert.ok(mark && mark.editorial.length === 6, id + ' missing score');
    assert.ok([...mark.editorial, mark.fidelity].every(score => [0, 1, 2].includes(score)), id);
    assert.equal(mark.fidelity === 2, mark.added.length + mark.inherited.length === 0, id);
    const content = read('calls/' + id + '/content.md');
    assert.ok(content.includes(mark.hook) && content.includes(mark.ending), id + ' passages');
  }
  return {id, reviewId: reviewId(id), topic, arm, fixture, stratum: config.fixtures[fixture], ...mark, input, cached, output, total, usd, noCacheUsd};
});
const normalize = (request, edit) => {
  for (const message of request.messages.filter(m => m.role === 'user')) {
    if (typeof message.content === 'string') message.content = edit(message.content);
    else for (const part of message.content.filter(p => p.type === 'text')) part.text = edit(part.text);
  }
  return request;
};
const base = rows.filter(row => row.arm === 'A');
for (const a of base) {
  const input = call(a.id, 'input');
  const request = call(a.id, 'request');
  assert.deepEqual(input.narrative.content, read('fixtures/' + a.fixture + '/content.md'));
  for (const arm of 'BC') {
    const candidate = rows.find(row => row.id === a.id.slice(0,-1) + arm);
    if (!candidate) continue;
    const changedInput = call(candidate.id,'input');
    const changedRequest = call(candidate.id,'request');
    if (arm === 'B') {
      const name = exists('direction/' + a.fixture + '.md') ? a.fixture : a.topic;
      const suffix = '\n\n' + read('direction/' + name + '.md').trimEnd();
      const expectedContent = input.narrative.content + suffix;
      assert.equal(changedInput.narrative.content, expectedContent);
      changedInput.narrative.content = input.narrative.content;
      assert.deepEqual(normalize(changedRequest, s => s.replace(expectedContent, input.narrative.content)), request, candidate.id);
    } else {
      assert.deepEqual(normalize(changedRequest, s => s.replace(editedLine, stockLine)), request, candidate.id);
    }
    assert.deepEqual(changedInput,input,candidate.id);
  }
}
if (verificationOnly) {
  console.log(rows.length + ' unieke voltooide responses. Armverschillen binnen complete blokken gecontroleerd.');
  return;
}
const sum = (items, get) => items.reduce((acc, item) => acc + get(item), 0);
const otherEditorial = row => sum(row.editorial.filter((_,i) => i !== 4), n => n);
const summarize = group => ({
  posts: group.length,
  fidelity: group.filter(r => r.fidelity === 2).length,
  inherited: group.filter(r => r.inherited.length).length,
  added: group.filter(r => r.added.length).length,
  otherEditorial: sum(group,otherEditorial),
  dimensionPasses: Array.from({length:6}, (_, i) => group.filter(r => r.editorial[i] === 2).length),
  ...Object.fromEntries(['input','cached','output','total','usd','noCacheUsd'].map(k => [k,sum(group,r => r[k])]))
});
const groups = Object.fromEntries([... 'ABC'].map(arm => [arm, rows.filter(r => r.arm === arm)]));
const overall = Object.fromEntries(Object.entries(groups).map(([arm,group]) => [arm,summarize(group)]));
const by = (key, values) => Object.fromEntries(values.map(value => [value, Object.fromEntries(Object.entries(groups).map(
  ([arm, group]) => [arm, summarize(group.filter(r => r[key] === value))]))]));
const byTopic = by('topic',topics);
const byStratum = by('stratum',['safe','problematic']);
const replay = exists('replay.txt') && read('replay.txt').includes(invalid + ': original failure replayed') && config.schedule.every(id => read('replay.txt').includes(id + ': complete'));
const comparisons = Object.fromEntries([... 'BC'].map(arm => {
  const paired = base.flatMap(a => {
    const b = groups[arm].find(r => r.id === a.id.slice(0,-1) + arm);
    return b ? [{id: a.id.slice(0,-2), ending:[a.editorial[4],b.editorial[4]], fidelity:[a.fidelity,b.fidelity],
      added:[a.added,b.added], inherited:[a.inherited,b.inherited], newRegressions:b.added.filter(issue => !a.added.includes(issue)),
      otherEditorial:[otherEditorial(a),otherEditorial(b)], tokenDelta:b.total-a.total, usdDelta:b.usd-a.usd, noCacheUsdDelta:b.noCacheUsd-a.noCacheUsd}] : [];
  });
  const a = overall.A;
  const b = overall[arm];
  const gates = {
    coverage: rows.length === 48 && paired.length === 16 && replay,
    endingAbsolute: b.dimensionPasses[4] >= 10,
    endingGain: b.dimensionPasses[4] - a.dimensionPasses[4] >= 4,
    endingPerCase: Object.values(byTopic).every(t => t[arm].dimensionPasses[4] >= 2),
    safeFidelity: byStratum.safe[arm].fidelity >= 7,
    noMoreNewErrors: b.added <= a.added,
    noPairedNewErrors: paired.every(p => p.newRegressions.length === 0),
    inherited: byStratum.problematic[arm].inherited <= byStratum.problematic.A.inherited,
    otherDimensions: b.dimensionPasses.every((n,i) => i === 4 || n >= 12) && b.otherEditorial >= a.otherEditorial,
    tokens: b.total <= 1.1*a.total,
    usd: b.usd <= 1.1*a.usd
  };
    const endingPairs = {
    better: paired.filter(p => p.ending[1] > p.ending[0]).length,
    equal: paired.filter(p => p.ending[1] === p.ending[0]).length,
    worse: paired.filter(p => p.ending[1] < p.ending[0]).length
  };
  return [arm,{decision:Object.values(gates).every(Boolean) ? 'GO':'NO-GO',gates,endingPairs,paired}];
}));
const decision = Object.values(comparisons).some(c => c.decision === 'GO') ? 'GO':'NO-GO';
const usage = invalidResponse.usage;
const input = usage.prompt_tokens;
const output = usage.completion_tokens;
const cached = usage.prompt_tokens_details?.cached_tokens ?? 0;
assert.equal(input + output, usage.total_tokens);
const invalidUsage = {input, cached, output, total: usage.total_tokens,
  usd: (rates.input * (input - cached) + rates.cached * cached + rates.output * output) / 1e6,
  noCacheUsd: (rates.input * input + rates.output * output) / 1e6};
const allAttemptsUsage = Object.fromEntries(Object.keys(invalidUsage).map(key =>
  [key, sum(rows, row => row[key]) + invalidUsage[key]]));
const withoutReplacement = Object.fromEntries(Object.entries(groups).map(([arm, group]) =>
  [arm, summarize(group.filter(row => row.id !== invalid))]));
const attempts = config.schedule.flatMap((id, index) => {
  const record = (directory, kind) => {
    const marker = path.join(directory, 'attempt.json');
    if (!fs.existsSync(marker)) return [];
    const load = name => {
      const file = path.join(directory, name);
      return fs.existsSync(file) ? JSON.parse(fs.readFileSync(file, 'utf8')) : null;
    };
    const status = load('http.json');
    const raw = load('response-raw.json');
    return [{position: index + 1, id, kind, directory: path.relative(__dirname, directory).replaceAll(path.sep, '/'),
      startedUtc: load('attempt.json').startedUtc, http: status?.status ?? null,
      responseId: raw?.id ?? null, finishReason: raw?.choices?.[0]?.finish_reason ?? null,
      valid: fs.existsSync(path.join(directory, 'result.json'))}];
  };
  const call = record(path.dirname(resolve('calls/' + id + '/attempt.json')),
    originalIds.has(id) ? 'original' : id === invalid ? 'replacement' : 'continuation');
  return id === invalid ? [...record(archive, 'original-invalid'), ...call] : call;
});
console.log(JSON.stringify({decision,overall,byTopic,byStratum,comparisons,attempts,
  invalidUsage,allAttemptsUsage,withoutReplacement,rows},null,2));

