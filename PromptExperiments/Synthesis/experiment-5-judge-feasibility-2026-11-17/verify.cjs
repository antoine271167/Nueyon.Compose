const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const assert = require('node:assert/strict');
const { execFileSync } = require('node:child_process');
process.chdir(__dirname);

const json = file => JSON.parse(fs.readFileSync(file, 'utf8'));
const hash = file => crypto.createHash('sha256').update(fs.readFileSync(file)).digest('hex');
function list(dir) {
  return fs.readdirSync(dir, { withFileTypes: true }).flatMap(entry => {
    const file = path.join(dir, entry.name);
    return entry.isDirectory() ? list(file) : [file];
  });
}
const manifestFile = 'artifact-manifest.json';
const files = list('.').filter(file => file !== manifestFile).map(file => file.replaceAll(path.sep, '/')).sort();
const hashes = Object.fromEntries(files.map(file => [file, hash(file)]));
if (process.argv[2] === 'seal') {
  fs.writeFileSync(manifestFile, JSON.stringify(hashes, null, 2) + '\n', { flag: 'wx' });
}
assert.deepEqual(hashes, json(manifestFile));
const freeze = json('freeze.json');
for (const [file, expected] of Object.entries(freeze.upstream)) assert.equal(hash(file), expected, file);
const lock = Date.parse(fs.readFileSync('run.lock', 'utf8'));
assert(Date.parse(freeze.at) <= lock);
for (const id of freeze.ids) {
  const dir = 'calls/' + id;
  const start = Date.parse(json(dir + '/attempt.json').started);
  const http = json(dir + '/http.json');
  assert(lock <= start && start <= Date.parse(http.at));
  assert.equal(http.status, 200);
}
const actual = execFileSync(process.execPath, ['evaluate.cjs'], { encoding: 'utf8' });
assert.deepEqual(JSON.parse(actual), json('evaluation.json'));
const result = JSON.parse(actual);
assert(result.totals.prompt <= freeze.inputBound && result.totals.completion <= freeze.completionBound);
console.log(JSON.stringify({ artifacts: files.length, references: Object.keys(freeze.upstream).length,
  attempts: freeze.ids.length, matrix: result.matrix, decision: result.decision }));
