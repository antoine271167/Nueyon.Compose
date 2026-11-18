const fs = require('node:fs');
const path = require('node:path');

const source = 'https://developers.openai.com/api/docs/models/gpt-6-luna';
const usdPerMillion = { input: 0.10, cachedInput: 0.01, output: 0.50 };
const { runs } = JSON.parse(fs.readFileSync(path.join(__dirname, 'usage.json'), 'utf8'));
const estimate = runs.map(({ id, usage }) => {
    const cachedInput = usage.prompt_tokens_details?.cached_tokens ?? 0;
    const input = usage.prompt_tokens - cachedInput;
    const output = usage.completion_tokens;
    const usd = (input * usdPerMillion.input + cachedInput * usdPerMillion.cachedInput
        + output * usdPerMillion.output) / 1_000_000;
    return { id, input, cachedInput, output, usd };
});
const total = selected => Number(selected.reduce((sum, run) => sum + run.usd, 0).toFixed(8));
const byArmUsd = Object.fromEntries(['A', 'B'].map(arm =>
    [arm, total(estimate.filter(run => run.id.endsWith('-' + arm)))]));
const result = { source, tariffDate: '2026-11-18', usdPerMillion, totalUsd: total(estimate), byArmUsd, runs: estimate };
fs.writeFileSync(path.join(__dirname, 'costs.json'), JSON.stringify(result, null, 2) + '\n');
console.log('Estimated Standard USD:', result.totalUsd, result.byArmUsd);
