const fs = require('fs');
const assert = require('node:assert/strict');
const path = require('node:path');

const read = name => JSON.parse(fs.readFileSync(path.join(__dirname, name), 'utf8'));
const plan = read('plan.json');
const scores = read('scores.json');
const requests = read('verification-requests.json');
const replay = read('verification-offline-replay.json');
const dims = ['BT', 'F', 'BN', 'IW', 'NG'];
const cases = ['webb', 'wolf', 'digital', 'lowell'];

assert.equal(Object.keys(scores).length, plan.schedule.length);
assert.equal(new Set(plan.schedule.map(lane => lane.id)).size, 24);

const rows = plan.schedule.map(lane => {
  const id = lane.id + '-Synthesis';
  const valid = requests.some(check => check.id === id && !Object.values(check).includes(false))
    && replay.some(check => check.id === id && check.parsedAndExact);
  const score = scores[lane.id];
  assert.equal(score.ratings.length, dims.length);
  assert(score.ratings.every(value => [0, 1, 2].includes(value)));
  assert.equal(typeof score.material, 'boolean');
  assert(Array.isArray(score.omissions));
  assert.equal(typeof score.note, 'string');
  if (lane.case === 'wolf' || lane.case === 'webb') assert.equal(typeof score.targetPass, 'boolean');
  if (lane.case === 'wolf') assert.equal(typeof score.concreteFood, 'boolean');
  return { ...lane, ...score, valid, pass: valid && score.ratings.every(value => value === 2) };
});

const arm = Object.fromEntries(['A', 'C'].map(label => {
  const selected = rows.filter(row => row.arm === label);
  return [label, {
    valid: selected.filter(row => row.valid).length,
    pass: selected.filter(row => row.pass).length,
    perCase: Object.fromEntries(cases.map(name => [
      name, selected.filter(row => row.case === name && row.pass).length
    ])),
    material: selected.filter(row => row.material).map(row => row.id),
    targetFailures: Object.fromEntries(['webb', 'wolf'].map(name => [
      name, selected.filter(row => row.case === name && row.targetPass === false).map(row => row.id)
    ]))
  }];
}));

const paired = dims.map((dimension, index) => {
  const differences = rows.filter(row => row.arm === 'A').map(a => {
    const c = rows.find(row => row.id === a.id.replace(/A$/, 'C'));
    return c.ratings[index] - a.ratings[index];
  });
  return {
    dimension,
    lower: differences.filter(delta => delta < 0).length,
    higher: differences.filter(delta => delta > 0).length
  };
});

const recurring = new Map();
for (const row of rows.filter(row => row.arm === 'C')) {
  for (const omission of new Set(row.omissions)) {
    const key = row.case + '/' + omission;
    recurring.set(key, (recurring.get(key) || 0) + 1);
  }
}
const repeatedOmissions = [...recurring].filter(([, count]) => count > 1);
const gates = {
  complete: arm.A.valid === 12 && arm.C.valid === 12,
  sufficient: arm.C.pass >= 11 && cases.every(name => arm.C.perCase[name] >= 2),
  noMaterialErrors: arm.C.material.length === 0,
  noRepeatedOmissions: repeatedOmissions.length === 0,
  noPassRegression: arm.C.pass >= arm.A.pass && cases.every(name => arm.C.perCase[name] >= arm.A.perCase[name]),
  noPairedDimensionRegression: paired.every(axis => axis.lower <= 1 || axis.higher >= axis.lower),
  correctWolfReferent: arm.C.targetFailures.wolf.length === 0,
  wolfFoodEvidence: rows.filter(row => row.arm === 'C' && row.case === 'wolf').every(row => row.concreteFood),
  retainedWebbReason: arm.C.targetFailures.webb.length === 0,
  observedWolfImprovement: arm.A.targetFailures.wolf.length > 0 && arm.C.targetFailures.wolf.length === 0,
  observedWebbImprovement: arm.A.targetFailures.webb.length > 0 && arm.C.targetFailures.webb.length === 0
};
const result = { decision: Object.values(gates).every(Boolean) ? 'GO vervolgvalidatie' : 'NO-GO', arm, paired, repeatedOmissions, gates };
fs.writeFileSync(path.join(__dirname, 'gate-evaluation.json'), JSON.stringify(result, null, 2) + '\n');
console.log(JSON.stringify(result, null, 2));

