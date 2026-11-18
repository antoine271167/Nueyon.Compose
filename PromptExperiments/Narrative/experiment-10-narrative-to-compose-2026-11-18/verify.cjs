const assert = require('node:assert/strict');
const fs = require('node:fs');
const config = require('./config.json');
const reviewId = require('./review.cjs');
const read = file => fs.readFileSync(__dirname + '/' + file, 'utf8');
const json = file => JSON.parse(read(file));
const call = (id, file) => json('calls/' + id + '/' + file + '.json');
const edit = read('variant/Compose/Compose.linkedin.user.prompt.md')
  .split(/\r?\n/).find(line => line.startsWith('In the LinkedIn draft,'));
assert.ok(edit);
const marks = Object.assign({}, ...['digital', 'webb', 'wolf', 'lowell'].map(topic => json('scores/' + topic + '.json')));
const responseIds = new Set();
const rates = config.ratesPerMillionUsd;
const rows = config.schedule.map(id => {
  const [, topic, arm] = id.match(/^([^-]+).*-([AB])$/);
  const fixture = id.replace(/-c[12]-[AB]$/, '');
  const response = call(id, 'response-raw');
  assert.equal(response.model, config.model);
  assert.equal(response.choices[0].finish_reason, 'stop');
  assert.ok(response.id && !responseIds.has(response.id));
  responseIds.add(response.id);
  const usage = response.usage;
  const input = usage.prompt_tokens;
  const cached = usage.prompt_tokens_details?.cached_tokens ?? 0;
  const output = usage.completion_tokens;
  const total = usage.total_tokens;
  assert.equal(total, input + output);
  const usd = (rates.input * (input - cached) + rates.cached * cached + rates.output * output) / 1e6;
  const mark = marks[reviewId(id)];
  assert.ok(mark && mark.editorial.length === 6);
  assert.ok([...mark.editorial, mark.fidelity].every(score => [0, 1, 2].includes(score)));
  return {id, reviewId: reviewId(id), topic, arm, fixture, stratum: config.fixtures[fixture], ...mark, input, cached, output, total, usd};
});
assert.equal(Object.keys(marks).length, rows.length);
const sum = (array, get) => array.reduce((acc, item) => acc + get(item), 0);
const soft = row => sum(row.editorial, n => n);
const paired = rows.filter(row => row.arm === 'A').map(a => {
  const b = rows.find(row => row.id === a.id.slice(0, -1) + 'B');
  assert.deepEqual(call(a.id, 'input'), call(b.id, 'input'));
  const requestA = call(a.id, 'request');
  const requestB = call(b.id, 'request');
  const userMessage = requestB.messages.find(message => message.role === 'user');
  const removeEdit = content => {
    const index = content.indexOf(edit);
    if (index === -1) return content;
    const rest = content.slice(index + edit.length);
    assert.ok(/^\r?\n\r?\n/.test(rest));
    return content.slice(0, index) + rest.replace(/^\r?\n\r?\n/, '');
  };
  if (typeof userMessage.content === 'string') {
    userMessage.content = removeEdit(userMessage.content);
  } else {
    for (const part of userMessage.content) {
      if (part.type === 'text') part.text = removeEdit(part.text);
    }
  }
  assert.deepEqual(requestA, requestB, a.id);
  return {id: a.id.slice(0, -2), topic: a.topic, stratum: a.stratum,
    fidelity: [a.fidelity, b.fidelity], editorial: [soft(a), soft(b)],
    newRegressions: b.added.filter(issue => !a.added.includes(issue)),
    inherited: [a.inherited, b.inherited], added: [a.added, b.added],
    tokenDelta: b.total - a.total, usdDelta: b.usd - a.usd};
});
const summarize = group => ({
  posts: group.length,
  fidelity: group.filter(row => row.fidelity === 2).length,
  inherited: group.filter(row => row.inherited.length).length,
  added: group.filter(row => row.added.length).length,
  editorial: sum(group, soft),
  editorialFull: group.filter(row => soft(row) === 12).length,
  dimensionPasses: Array.from({length: 6}, (_, i) => group.filter(row => row.editorial[i] === 2).length),
  ...Object.fromEntries(['input', 'cached', 'output', 'total', 'usd'].map(key => [key, sum(group, row => row[key])]))
});
const arm = label => rows.filter(row => row.arm === label);
const a = summarize(arm('A'));
const b = summarize(arm('B'));
const byTopic = Object.fromEntries(['digital', 'webb', 'wolf', 'lowell'].map(topic =>
  [topic, ['A', 'B'].map(label => summarize(arm(label).filter(row => row.topic === topic)))]));
const byStratum = Object.fromEntries(['safe', 'problematic'].map(stratum =>
  [stratum, ['A', 'B'].map(label => summarize(arm(label).filter(row => row.stratum === stratum)))]));
const gates = {
  coverage: paired.length === 16,
  fewerNewErrors: a.added - b.added >= 2,
  noNewPairedErrors: paired.every(pair => pair.newRegressions.length === 0),
  safeFidelity: byStratum.safe[1].fidelity >= 7,
  inherited: byStratum.problematic[1].inherited <= byStratum.problematic[0].inherited,
  editorialOverall: b.editorialFull >= 10,
  editorialPerTopic: Object.values(byTopic).every(([_, vb]) => vb.editorialFull >= 2),
  noAggregateEditorialLoss: b.editorial >= a.editorial && Object.values(byTopic).every(([va, vb]) => vb.editorial >= va.editorial),
  maxThreeEditorialRegressions: paired.filter(pair => pair.editorial[1] < pair.editorial[0]).length <= 3,
  totalTokens: b.total <= a.total * 1.1,
  estimatedCost: b.usd <= a.usd * 1.1
};
console.log(JSON.stringify({decision: Object.values(gates).every(Boolean) ? 'GO' : 'NO-GO', gates, a, b, byTopic, byStratum, paired, rows}, null, 2));
