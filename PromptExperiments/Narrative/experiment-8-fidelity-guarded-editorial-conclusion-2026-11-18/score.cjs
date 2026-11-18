const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const stage = process.argv[2] ?? 'phase1';
assert(['phase1', 'phase2'].includes(stage));
const read = name => JSON.parse(fs.readFileSync(path.join(__dirname, name), 'utf8'));
const write = (name, value) => fs.writeFileSync(path.join(__dirname, name), JSON.stringify(value, null, 2) + '\n');
const config = read('config.json');
const usage = read(stage + '-usage.json');
const manual = read(stage + '-scores.json');
const dimensions = ['focus', 'conclusion', 'structure', 'details', 'fidelity', 'handoff'];
const ids = config[stage];
assert.deepEqual(Object.keys(manual).sort(), [...ids].sort());
const scores = Object.fromEntries(ids.map(id => {
    assert.equal(manual[id].scores.length, dimensions.length);
    assert.equal(manual[id].evidence.length, dimensions.length);
    assert(manual[id].scores.every(score => [0, 1, 2].includes(score)));
    assert(manual[id].evidence.every(note => note.trim().length > 0));
    assert(Array.isArray(manual[id].materialErrorTypes));
    return [id, Object.fromEntries(dimensions.map((dimension, index) => [dimension, manual[id].scores[index]]))];
}));

const summary = {};
const caseNames = [...new Set(ids.map(id => id.split('-')[0]))];
const complete = id => dimensions.every(dimension => scores[id][dimension] === 2);
for (const arm of ['F', 'G']) {
    const armIds = ids.filter(id => id.endsWith('-' + arm));
    summary[arm] = {
        scoresOfTwo: Object.fromEntries(dimensions.map(d => [d, armIds.filter(id => scores[id][d] === 2).length])),
        sums: Object.fromEntries(dimensions.map(d => [d, armIds.reduce((sum, id) => sum + scores[id][d], 0)])),
        complete: armIds.filter(complete).length,
        completeByCase: Object.fromEntries(caseNames.map(name => [name, armIds.filter(id => id.startsWith(name + '-') && complete(id)).length])),
        materialErrorTypes: [...new Set(armIds.flatMap(id => manual[id].materialErrorTypes))]
    };
}

const drops = [];
for (const id of ids.filter(id => id.endsWith('-G'))) {
    const control = id.slice(0, -1) + 'F';
    for (const dimension of dimensions) {
        if (scores[id][dimension] < scores[control][dimension]) {
            drops.push({ control, id, dimension, from: scores[control][dimension], to: scores[id][dimension] });
        }
    }
}
const { F, G } = summary;
const tokenRatio = usage.totals['Narrative-G'].total / usage.totals['Narrative-F'].total;
const casePass = (name, dimension) => ids.filter(id => id.startsWith(name + '-') && id.endsWith('-G'))
    .every(id => scores[id][dimension] === 2);
const replay = fs.readFileSync(path.join(__dirname, 'replay-' + stage + '.txt'), 'utf8');
const criteria = {
    verifiedCallsAndExactReplay: usage.rows.length === ids.length && ids.every(id => replay.includes(id + ': complete')),
    ...(stage === 'phase1' ? {
        digitalFocus: casePass('digital', 'focus'),
        digitalFidelity: casePass('digital', 'fidelity'),
        webbFidelity: casePass('webb', 'fidelity')
    } : { allFidelity: G.scoresOfTwo.fidelity === 6 }),
    allStructure: G.scoresOfTwo.structure === 6,
    fiveConclusions: G.scoresOfTwo.conclusion >= 5,
    fiveFullySufficient: G.complete >= 5,
    twoFullySufficientPerCase: Object.values(G.completeByCase).every(count => count >= 2),
    noFocusSumDecline: G.sums.focus >= F.sums.focus,
    noConclusionSumDecline: G.sums.conclusion >= F.sums.conclusion,
    fidelitySum: G.sums.fidelity >= F.sums.fidelity && (stage === 'phase2' || F.sums.fidelity === 12 || G.sums.fidelity > F.sums.fidelity),
    noPairedDropToZero: drops.every(drop => drop.to !== 0),
    noNewMaterialErrorType: G.materialErrorTypes.every(type => F.materialErrorTypes.includes(type)),
    withinTokenLimit: tokenRatio <= 1.10
};
const decision = criteria.verifiedCallsAndExactReplay
    ? (Object.values(criteria).every(Boolean) ? 'GO' : 'NO-GO')
    : 'TECHNICALLY INCOMPLETE';
const costByArm = {};
const costRows = usage.rows.map(row => {
    const cached = row.cached ?? 0;
    const usd = ((row.input - cached) * 0.10 + cached * 0.01 + row.completion * 0.50) / 1000000;
    costByArm[row.arm] = (costByArm[row.arm] ?? 0) + usd;
    return { id: row.id, usd };
});
write(stage + '-costs.json', {
    source: 'https://developers.openai.com/api/docs/pricing',
    standardUsdPerMillion: { input: 0.10, cachedInput: 0.01, output: 0.50 },
    estimated: true, byArm: costByArm, rows: costRows
});
write(stage + '-decision.json', { decision, criteria, summary, tokenRatio, drops });
console.log(JSON.stringify({ decision, criteria, summary, tokenRatio, costByArm }, null, 2));

