const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const read = name => JSON.parse(fs.readFileSync(path.join(__dirname, name), 'utf8'));
const stage = process.argv[2] ?? 'phase1';
assert(['phase1', 'phase2'].includes(stage));
const schedule = read('config.json')[stage];
const scores = read(stage + '-scores.json');
const tokens = read(stage + '-usage.json').totals;
const replay = fs.readFileSync(path.join(__dirname, 'replay-' + stage + '.txt'), 'utf8');
const axes = ['focus', 'progression', 'details', 'fidelity', 'handoff'];

assert.deepEqual(Object.keys(scores).sort(), [...schedule].sort());
for (const id of schedule) {
    const score = scores[id];
    for (const axis of axes) assert([0, 1, 2].includes(score[axis]));
    assert(typeof score.evidence === 'string' && score.evidence.length > 0);
    assert(typeof score.novelMaterialError === 'boolean');
}
const arm = letter => schedule.filter(id => id.endsWith('-' + letter));
const b = arm('B');
const e = arm('E');
const full = id => axes.every(axis => scores[id][axis] === 2);
const sum = (ids, axis) => ids.reduce((total, id) => total + scores[id][axis], 0);
const maxima = (ids, axis) => ids.filter(id => scores[id][axis] === 2).length;
const cases = [...new Set(schedule.map(id => id.split('-')[0]))];

const pairedDeclines = [];
for (const id of e) {
    const control = id.slice(0, -1) + 'B';
    for (const axis of axes) {
        if (scores[id][axis] < scores[control][axis]) {
            pairedDeclines.push({ id, control, axis, from: scores[control][axis], to: scores[id][axis] });
        }
    }
}
const counts = Object.fromEntries([['B', b], ['E', e]].map(([label, ids]) => [
    label,
    {
        full: ids.filter(full).length,
        completeByAxis: Object.fromEntries(axes.map(axis => [axis, maxima(ids, axis)])),
        sumByAxis: Object.fromEntries(axes.map(axis => [axis, sum(ids, axis)])),
        fullByCase: Object.fromEntries(cases.map(name => [name, ids.filter(id => id.startsWith(name + '-') && full(id)).length]))
    }
]));
const enoughImprovement = axis => {
    const baseline = sum(b, axis);
    return sum(e, axis) >= baseline + Number(stage === 'phase1' && baseline < b.length * 2);
};
const criteria = {
    completeReplay: schedule.every(id => replay.includes(id + ': complete')),
    digitalFocus: stage !== 'phase1' || e.filter(id => id.startsWith('digital-') && scores[id].focus === 2).length === 3,
    webbFidelity: stage !== 'phase1' || e.filter(id => id.startsWith('webb-') && scores[id].fidelity === 2).length === 3,
    focusCount: counts.E.completeByAxis.focus >= 5,
    focusImprovement: enoughImprovement('focus'),
    fidelityImprovement: enoughImprovement('fidelity'),
    fullCount: counts.E.full >= 5,
    perCase: Object.values(counts.E.fullByCase).every(count => count >= 2),
    noNewZero: pairedDeclines.every(change => change.to !== 0),
    noNovelMaterialError: e.every(id => !scores[id].novelMaterialError),
    tokenBudget: tokens['Narrative-E'].total * 100 <= tokens['Narrative-B'].total * 110
};
const decision = Object.values(criteria).every(Boolean) ? 'GO' : 'NO-GO';
fs.writeFileSync(path.join(__dirname, stage + '-decision.json'),
    JSON.stringify({ decision, criteria, counts, pairedDeclines, tokens }, null, 2) + '\n');
console.log(decision);
