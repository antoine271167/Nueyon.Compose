const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root = __dirname;
const config = require('./config.json');

function reviewId(run) {
  return crypto.createHash('sha256').update('exp10-review-v1/' + run).digest('hex').slice(0, 8);
}

const rows = config.schedule.filter(run => !process.argv[2] || run.startsWith(process.argv[2] + '-'))
  .filter(run => !process.argv[3] || run.includes('-c' + process.argv[3] + '-'))
  .map(run => ({
    run,
    id: reviewId(run),
    file: path.join(root, 'calls', run, 'content.md'),
    fixture: run.replace(/-c[12]-[AB]$/, ''),
  })).sort((a, b) => a.id.localeCompare(b.id));

if (require.main === module) {
  if (rows.some(row => !fs.existsSync(row.file))) {
    console.error('Wait for all posts in this review batch.');
    process.exit(1);
  }
  for (const row of rows) {
    console.log(`\n## ${row.id} · ${row.fixture}\n`);
    console.log(fs.readFileSync(row.file, 'utf8'));
  }
}
module.exports = reviewId;
