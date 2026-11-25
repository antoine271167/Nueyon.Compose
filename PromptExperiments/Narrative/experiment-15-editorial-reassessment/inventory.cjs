const fs = require('node:fs');
const crypto = require('node:crypto');
const path = require('node:path');
const root = path.resolve(__dirname, '../../..');
const base = 'PromptExperiments/Narrative';
const read = file => fs.readFileSync(path.join(root, file));
const hash = value => crypto.createHash('sha256').update(value).digest('hex');
const json = file => JSON.parse(read(file).toString().replace(/^\uFEFF/, ''));
const rows = [];
const seen = new Map();
for (const [experiment, directory] of [
  [13, 'experiment-13-fidelity-and-resolution'],
  [14, 'experiment-14-evidence-anchored-narrative'],
]) {
  const folder = `${base}/${directory}`;
  for (const line of read(`${folder}/run-manifest.csv`).toString().trim().split(/\r?\n/).slice(1)) {
    const [run, subject, repeat, variant] = line.replaceAll('"', '').split(',');
    const output = `${folder}/runs/${run}.json`;
    const fixture = `${folder}/cases/${subject}`;
    const id = hash(`editorial-reassessment-15:${experiment}:${run}`).slice(0, 10);
    const row = { id, experiment, variant, subject, repeat, run, output,
      idea: `${fixture}/idea.json`, synthesis: `${fixture}/synthesis-result.json`,
      source: `${fixture}/source.md`, sha256: '', status: 'eligible', duplicateOf: '', missingReferences: '' };
    try {
      row.sha256 = hash(read(output));
      if (!json(output).content?.trim()) throw new Error('Missing content');
      if (seen.has(row.sha256)) {
        row.status = 'duplicate';
        row.duplicateOf = seen.get(row.sha256);
      } else seen.set(row.sha256, id);
    } catch {
      row.status = 'missing or invalid output';
    }
    row.missingReferences = ['idea', 'synthesis', 'source'].filter(key => {
      try { return !read(row[key]).length; } catch { return true; }
    }).join('|');
    rows.push(row);
  }
}
rows.sort((a, b) => a.id.localeCompare(b.id));
function csv(data) {
  const fields = Object.keys(data[0]);
  return [fields.join(','), ...data.map(row => fields.map(field =>
    '"' + String(row[field]).replaceAll('"', '""') + '"').join(','))].join('\n') + '\n';
}
if (require.main === module) {
  const [mode, subject] = process.argv.slice(2);
  if (mode === 'manifest') process.stdout.write(csv(rows));
  else {
    const cohort = rows.filter(row => row.subject === subject && row.status === 'eligible');
    if (mode === 'evidence') {
      for (const key of ['idea', 'synthesis', 'source']) {
        const versions = new Map(cohort.map(row => [hash(read(row[key])), read(row[key]).toString()]));
        for (const [digest, value] of versions) {
          const ids = cohort.filter(row => hash(read(row[key])) === digest).map(row => row.id);
          console.log(key, ids.join(' '), value);
        }
      }
    } else if (mode === 'review') {
      for (const row of cohort) console.log(row.id + '\n' + json(row.output).content + '\n');
    } else console.log(rows.reduce((counts, row) =>
      ({ ...counts, [row.status]: (counts[row.status] ?? 0) + 1 }), {}));
  }
}
module.exports = { rows, read, csv, hash };
