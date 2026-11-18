const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const read = name => fs.readFileSync(path.join(__dirname, name), 'utf8').replace(/^\uFEFF/, '');
const { schedule } = JSON.parse(read('config.json'));
const { totals } = JSON.parse(read('usage.json'));

function load(filename, expected, dimensions) {
    const [header, ...lines] = read(filename).trim().split(/\r?\n/);
    assert.deepEqual(header.split(',').slice(1, dimensions.length + 1), dimensions);
    const scores = new Map();
    for (const line of lines) {
        const [id, ...fields] = line.split(',');
        const values = fields.slice(0, dimensions.length).map(Number);
        assert(expected.includes(id) && !scores.has(id));
        assert.equal(values.length, dimensions.length);
        assert(values.every(value => [0, 1, 2].includes(value)));
        assert(fields.slice(dimensions.length).join(',').trim());
        scores.set(id, values);
    }
    assert.equal(scores.size, expected.length);
    return scores;
}

function assess(scores, dimensions) {
    const arms = {};
    const pairedRegressions = [];
    for (const [id, values] of scores) {
        const arm = id.at(-1);
        const block = id.slice(0, -1);
        const caseName = id.replace(/^linkedin-/, '').split('-')[0];
        const summary = arms[arm] ??= { sufficient: 0, cases: {}, byDimension: {} };
        const sufficient = values.every(value => value === 2);
        summary.sufficient += Number(sufficient);
        summary.cases[caseName] = (summary.cases[caseName] ?? 0) + Number(sufficient);
        dimensions.forEach((dimension, index) => {
            summary.byDimension[dimension] = (summary.byDimension[dimension] ?? 0) + Number(values[index] === 2);
            if (arm !== 'A') {
                for (const base of arm === 'C' ? ['A', 'B'] : ['A']) {
                    const before = scores.get(block + base)[index];
                    if (before > values[index]) pairedRegressions.push({ id, base, dimension, before, after: values[index] });
                }
            }
        });
    }
    return { arms, pairedRegressions };
}

const narrativeDimensions = ['focus', 'progression', 'details', 'fidelity', 'handoff'];
const linkedInDimensions = ['appeal', 'readability', 'message', 'accuracy'];
const narrative = load('narrative-scores.csv', schedule.slice(0, 36), narrativeDimensions);
const linkedIn = load('linkedin-scores.csv', schedule.slice(36), linkedInDimensions);
const n = assess(narrative, narrativeDimensions);
const l = assess(linkedIn, linkedInDimensions);
const { A, B, C } = n.arms;
const gates = {
    complete: schedule.every(id => fs.existsSync(path.join(__dirname, 'calls', id, 'result.json')))
        && (read('replay.txt').match(/: complete\s*$/gm) ?? []).length === 48,
    sufficient: C.sufficient >= 10 && C.sufficient >= B.sufficient + 2 && C.sufficient >= A.sufficient
        && Object.values(C.cases).every(count => count >= 2),
    focus: C.byDimension.focus >= 11 && C.byDimension.focus >= A.byDimension.focus
        && (C.byDimension.focus > B.byDimension.focus || C.byDimension.focus === 12),
    progressionDetailsHandoff: ['progression', 'details', 'handoff'].every(key => C.byDimension[key] >= B.byDimension[key]),
    fidelity: C.byDimension.fidelity === 12,
    paired: !n.pairedRegressions.some(row => row.id.endsWith('-C') && row.before === 2 && row.after === 0),
    tokens: totals['Narrative-C'].total <= 1.15 * totals['Narrative-B'].total
        && totals['Narrative-C'].total <= 0.5 * totals['Narrative-A'].total,
    linkedIn: linkedInDimensions.every(key => l.arms.C.byDimension[key] >= Math.max(l.arms.A.byDimension[key], l.arms.B.byDimension[key]))
        && l.arms.C.byDimension.message >= 3
        && [...linkedIn].filter(([id]) => id.endsWith('-C')).every(([, scores]) => scores[3] > 0)
};
const verdict = Object.values(gates).every(Boolean) ? 'GO' : 'NO-GO';
const result = { verdict, gates, narrative: n, linkedIn: l };
fs.writeFileSync(path.join(__dirname, 'score-summary.json'), JSON.stringify(result, null, 2) + '\n');
console.log(JSON.stringify(result, null, 2));
