const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const read = (name) => fs.readFileSync(path.join(__dirname, name), 'utf8').replace(/^\uFEFF/, '');
const [header, ...lines] = read('run-manifest.csv').trim().split(/\r?\n/);
assert.equal(header, '"id","case","repeat","variant"');
const manifest = lines.map((line) => {
    const [id, subject, repeat, variant] = line.replace(/"/g, '').split(',');
    return { id, subject, repeat: Number(repeat), variant };
});
const rubric = ['fidelity', 'focus', 'structure', 'tension', 'resolution', 'details', 'handoff', 'neutrality'];
const review = JSON.parse(read('reviews.json'));
const cases = ['digital', 'webb', 'wolf', 'lowell', 'cooling', 'library'];
const regression = cases.slice(0, 4);
assert.equal(manifest.length, 36);
assert.equal(new Set(manifest.map((row) => row.id)).size, 36);
assert.equal(Object.keys(review).length, 36);

const rows = manifest.map((row) => {
    const { id } = row;
    const { content } = JSON.parse(read('runs/' + id + '.json'));
    const status = JSON.parse(read('runs/' + id + '.status.json'));
    assert.equal(status.exitCode, 0);
    assert.equal(status.resultExists, true);
    assert.equal(status.variant, row.variant);
    assert.equal(status.case, row.subject);
    assert.equal(Number(status.repeat), row.repeat);
    assert.ok(content.trim());
    const failures = review[id];
    for (const failure of failures) {
        assert.ok(rubric.includes(failure.criterion));
        assert.ok(content.includes(failure.excerpt), id + ' missing output excerpt: ' + failure.excerpt);
        assert.ok(read(failure.source).includes(failure.evidence), id + ' missing source excerpt');
    }
    const scores = Object.fromEntries(rubric.map((criterion) => [
        criterion, Number(!failures.some((failure) => failure.criterion === criterion))
    ]));
    return {
        ...row, ...scores,
        full: Number(Object.values(scores).every(Boolean)),
        material: Number(failures.some((failure) => failure.criterion === 'fidelity' && failure.severity === 'material')),
    };
});
const metrics = [...rubric, 'full', 'material'];
const aggregate = (subset) => Object.fromEntries(['A', 'B'].map((variant) => {
    const selected = subset.filter((row) => row.variant === variant);
    return [variant, Object.fromEntries(metrics.map((metric) => [
        metric, selected.reduce((sum, row) => sum + row[metric], 0)
    ]))];
}));
const totals = aggregate(rows);
const bySubject = Object.fromEntries(cases.map((subject) => [
    subject, aggregate(rows.filter((row) => row.subject === subject))
]));
const fixed = aggregate(rows.filter((row) => regression.includes(row.subject)));
const additional = aggregate(rows.filter((row) => !regression.includes(row.subject)));
const gates = {
    zeroMaterialErrors: totals.B.material === 0,
    regressionMinimum: fixed.B.full >= 10,
    perSubjectMinimum: regression.every((subject) => bySubject[subject].B.full >= 2),
    resolutionMinimum: totals.B.resolution >= 15,
    regressionGain: fixed.B.full >= fixed.A.full + 2,
    resolutionGain: totals.B.resolution >= totals.A.resolution + 2,
    noOtherEditorialRegression: ['focus', 'structure', 'tension', 'details', 'handoff']
        .every((metric) => totals.B[metric] >= totals.A[metric]),
    formatNeutrality: totals.A.neutrality + totals.B.neutrality === 36,
    completeVerifiedCoverage: true,
};
const decision = Object.values(gates).every(Boolean) ? 'GO' : 'NO-GO';
const columns = ['id', 'subject', 'repeat', 'variant', ...metrics];
fs.writeFileSync(path.join(__dirname, 'scores.csv'), columns.join(',') + '\n' +
    rows.map((row) => columns.map((column) => row[column]).join(',')).join('\n') + '\n');
const result = { decision, gates, totals, fixed, additional, bySubject };
fs.writeFileSync(path.join(__dirname, 'decision.json'), JSON.stringify(result, null, 2) + '\n');
console.log(JSON.stringify(result));

