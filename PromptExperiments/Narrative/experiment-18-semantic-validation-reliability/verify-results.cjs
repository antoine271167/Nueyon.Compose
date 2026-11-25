const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const {verify} = require('./run.cjs');
const {parse} = require('../experiment-17-semantic-validation/run.cjs');
const read = name => fs.readFileSync(path.join(__dirname,name),'utf8');
const json = name => JSON.parse(read(name));
verify();
const ground = json('ground-truth.json');
const inputs = json('inputs.json');
const sources = json('sources.json');
const metrics = json('metrics.json');
let valid = 0;
let seq = 0;
const incomplete = [];
for (const repetition of [1,2]) {
  const cohort = repetition === 1 ? ground : [...ground].reverse();
  for (const item of cohort) {
    const conditions = (Number(item.id.slice(1))+repetition)%2 ? ['S+Source','S'] : ['S','S+Source'];
    for (const condition of conditions) {
      seq++;
      const base = ['runs',item.id,condition,repetition].join('/')+'/';
      if (!fs.existsSync(path.join(__dirname,base+'attempt.json'))) {
        incomplete.push(seq);
        continue;
      }
      const expected = {...inputs[item.id]};
      if (condition==='S+Source') expected.source=sources[item.subject].source;
      assert.deepEqual(json(base+'input.json'),expected);
      const meta=json(base+'attempt.json');
      assert.deepEqual([meta.seq,meta.id,meta.condition,meta.repetition],[seq,item.id,condition,repetition]);
      read(base+'stderr.log');
      const response=read(base+'response.txt');
      if (meta.valid) {
        assert.equal(parse(response).status,meta.status);
        valid++;
      } else {
        assert(meta.validationError);
        if (meta.exitCode===0 && !meta.validationError.startsWith('Transport exited:')) assert.throws(()=>parse(response));
      }
    }
  }
}
assert.equal(seq,metrics.planned);
assert.equal(seq-incomplete.length,metrics.attempted);
assert.equal(valid,metrics.valid);
console.log('Verified',valid,'valid archived responses out of',seq-incomplete.length,'attempts,',incomplete.length,'unattempted.');

