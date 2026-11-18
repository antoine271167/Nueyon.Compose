const fs = require('node:fs');
const report = require('./decision.json');
const label = issues => issues.length ? issues.join(', ') : '—';
const escape = s => s.replaceAll('|', '\\|').replace(/\r?\n/g, '<br>');
const table = [
  '# Herkomstmatrix per post',
  '',
  'De [audit vóór de calls](precall-audit.md) koppelt de oorspronkelijke briefpassages aan de bevroren Synthesis. Overgenomen en nieuw/versterkt omvatten ook mogelijk materiële implicaties. Scores en passages staan ook in [decision.json](decision.json).',
  '',
  '| Post | Stratum | Brontrouw | Overgenomen | Nieuw/versterkt | Bewijs en beoordeling |',
  '|---|---|---:|---|---|---|'
];
for (const row of report.rows) {
  const cells = [
    '[' + row.id + '](calls/' + row.id + '/content.md)',
    row.stratum,
    row.fidelity,
    label(row.inherited),
    label(row.added),
    escape(row.note)
  ];
  table.push('| ' + cells.join(' | ') + ' |');
}
fs.writeFileSync(__dirname + '/origin-matrix.md', table.join('\n') + '\n');
