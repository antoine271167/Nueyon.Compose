const fs = require('node:fs');
const assert = require('node:assert/strict');
const { execFileSync } = require('node:child_process');
process.chdir(__dirname);

const replay = JSON.parse(execFileSync(process.execPath, ['runner.cjs', 'replay'], { encoding: 'utf8' }));
const annotations = JSON.parse(fs.readFileSync('feedback-review.json', 'utf8'));
const rows = replay.rows.map(row => {
  const reviews = annotations[row.id] ?? [];
  assert.equal(reviews.length, row.blockers?.length ?? 0, row.id);
  const blockers = reviews.map((review, index) => {
    const blocker = row.blockers[index];
    return { ...review, actionable: review.supported && review.minimal && review.fieldCorrect &&
      blocker.exactOutput && blocker.exactEvidence };
  });
  return { ...row, reviews: blockers, detected: Boolean(row.target) && blockers.some(b => b.target === row.target),
    actionableTarget: Boolean(row.target) && blockers.some(b => b.target === row.target && b.actionable) };
});
assert(Object.keys(annotations).every(id => rows.some(row => row.id === id)));
const primary = rows.filter(row => row.experiment === 4);
const historical = rows.filter(row => row.experiment === 2);
const blockers = rows.flatMap(row => row.reviews);
const actionable = blockers.filter(b => b.actionable).length;
const species = primary.filter(row => row.target === 'species');
const folding = primary.filter(row => row.target === 'folding_reason');
const timeline = primary.filter(row => row.target === 'timeline');
const lowell = primary.filter(row => row.run.startsWith('lowell'));
const actor = historical.find(row => row.target === 'actor');
const strayBrand = historical.find(row => row.run === 'webb-r1-A');
const conditions = {
  all29Valid: rows.length === 29 && rows.every(row => row.class !== 'invalid'),
  atLeast5Of6TP: replay.matrix.primary.TP >= 5,
  atLeast17Of18TN: replay.matrix.primary.TN >= 17,
  bothSpeciesActionable: species.length === 2 && species.every(row => row.actionableTarget),
  primaryFoldingActionable: folding.length === 1 && folding.every(row => row.actionableTarget),
  atLeast2Of3TimelineActionable: timeline.filter(row => row.actionableTarget).length >= 2,
  all6LowellPass: lowell.length === 6 && lowell.every(row => row.verdict === 'PASS'),
  historicalActorActionable: actor?.actionableTarget === true,
  strayBrandNotBlocked: strayBrand?.class !== 'invalid' && !strayBrand?.reviews.some(review => review.target === 'stray_brand'),
  atLeast4Of5HistoricalActionable: historical.filter(row => row.actionableTarget).length >= 4,
  atLeast90PercentActionable: blockers.length > 0 && actionable / blockers.length >= 0.9,
  noMaterialRegressionProposed: blockers.every(b => !b.introducesMaterialError)
};
console.log(JSON.stringify({
  decision: Object.values(conditions).every(Boolean) ? 'GO' : 'NO-GO', conditions,
  matrix: replay.matrix, totals: replay.totals,
  feedback: { actionable, total: blockers.length }, rows
}, null, 2));
