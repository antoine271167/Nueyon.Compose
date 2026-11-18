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
            if (arm === 'D') {
                for (const base of ['B']) {
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
const narrative = load('narrative-scores.csv', schedule.slice(0, 24), narrativeDimensions);
const linkedIn = load('linkedin-scores.csv', schedule.slice(24), linkedInDimensions);
const n = assess(narrative, narrativeDimensions);
const l = assess(linkedIn, linkedInDimensions);
const { B, D } = n.arms;
const linked = l.arms.D.byDimension;
const fidelitySum = arm => [...narrative].filter(([id]) => id.endsWith(arm))
    .reduce((sum, [, scores]) => sum + scores[3], 0);
const evidence = JSON.parse(read('gate-evidence.json'));
const gates = {
    complete: schedule.every(id => fs.existsSync(path.join(__dirname, 'calls', id, 'result.json')))
        && (read('replay.txt').match(/: complete\s*$/gm) ?? []).length === 32,
    sufficient: D.sufficient >= 10 && Object.values(D.cases).every(count => count >= 2),
    focus: D.byDimension.focus >= 11 && D.byDimension.focus >= B.byDimension.focus
        && [1, 2, 3].every(round => narrative.get(`digital-r${round}-D`)[0] === 2),
    progressionDetailsHandoff: ['progression', 'details', 'handoff'].every(key => D.byDimension[key] === 12),
    fidelity: D.byDimension.fidelity >= B.byDimension.fidelity && fidelitySum('-D') >= fidelitySum('-B'),
    noNewMaterialFidelityError: evidence.newMaterialFidelityErrors.length === 0,
    paired: !n.pairedRegressions.some(row => row.after === 0),
    tokens: totals['Narrative-D'].total <= 1.10 * totals['Narrative-B'].total,
    linkedIn: linked.message === 4 && linked.accuracy === 4 && linked.appeal >= 3 && linked.readability >= 3
        && !l.pairedRegressions.some(row => row.after === 0)
};
const verdict = Object.values(gates).every(Boolean) ? 'GO' : 'NO-GO';
const result = { verdict, gates, narrative: n, linkedIn: l, evidence };
fs.writeFileSync(path.join(__dirname, 'score-summary.json'), JSON.stringify(result, null, 2) + '\n');
console.log(JSON.stringify(result, null, 2));
