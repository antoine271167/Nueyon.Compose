const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const load = file => JSON.parse(fs.readFileSync(path.join(__dirname, file), 'utf8'));
const exists = file => fs.existsSync(path.join(__dirname, file));
const config = load('config.json');
const narrative = load('narrative-scores.json');
const posts = load('linkedin-scores.json');
const initial = load('narrative-scores-initial-gate.json');
const responses = new Set();
const metrics = {};
const usage = [];
const rates = { input: 0.10, cached: 0.01, output: 0.50 };
const gate = {};

for (const id of config.schedule) {
    const folder = 'calls/' + id;
    const downstream = id.startsWith('linkedin-');
    const brief = downstream ? id.slice(9) : id;
    if (downstream) {
        gate[id] = narrative[brief]?.fidelity === 2 ? 'eligible' : 'blocked';
        if (gate[id] === 'blocked') {
            if (!exists(folder + '/attempt.json')) continue;
            assert.equal(initial[brief].fidelity, 2, id);
            assert(posts[id].fidelity < 2, id);
            gate[id] = 'executed before fidelity correction';
        }
    }
    if (!exists(folder + '/result.json')) continue;
    assert((downstream ? posts : narrative)[id], id);
    const request = load(folder + '/request.json');
    const response = load(folder + '/response-raw.json');
    const input = load(folder + '/input.json');
    assert.equal(request.model, config.model);
    assert.equal(response.model, config.model);
    assert.equal(response.choices[0].finish_reason, 'stop');
    assert.equal(load(folder + '/http.json').status, 200);
    assert(!responses.has(response.id));
    responses.add(response.id);
    assert(!request.previous_response_id && !request.conversation, id);
    assert.equal(request.messages.length, 2, id);
    const { messages, ...settings } = request;
    const stage = downstream ? 'linkedin' : 'narrative';
    metrics[stage] ??= settings;
    assert.deepEqual(settings, metrics[stage], id);
    if (downstream) {
        assert(messages[1].content.includes(load('calls/' + brief + '/result.json').content), id);
    } else {
        assert.deepEqual(input, load('cases/' + id.split('-')[0] + '/input.json'), id);
        const other = id.slice(0, -1) + (id.endsWith('F') ? 'H' : 'F');
        if (exists('calls/' + other + '/request.json')) {
            assert.deepEqual(messages[1], load('calls/' + other + '/request.json').messages[1], id);
        }
    }
    const u = response.usage;
    const cached = u.prompt_tokens_details?.cached_tokens ?? 0;
    assert.equal(u.total_tokens, u.prompt_tokens + u.completion_tokens);
    usage.push({
        id, stage, arm: id.at(-1), input: u.prompt_tokens, cached,
        output: u.completion_tokens, total: u.total_tokens,
        usd: ((u.prompt_tokens - cached) * rates.input + cached * rates.cached +
            u.completion_tokens * rates.output) / 1_000_000
    });
}

const save = (file, value) => fs.writeFileSync(path.join(__dirname, file), JSON.stringify(value, null, 2) + '\n');
const softN = ['focus', 'progression', 'evidence', 'direction', 'handoff'];
const softP = ['appeal', 'readability', 'message', 'ending', 'handoff'];
for (const [scores, keys] of [[narrative, softN], [posts, softP]]) {
    for (const row of Object.values(scores)) {
        for (const k of [...keys, 'fidelity']) assert([0, 1, 2].includes(row[k]));
        assert(row.evidenceNotes);
    }
}
const summaries = {};
const cases = ['digital', 'webb', 'wolf', 'lowell'];
for (const [stage, scores, keys] of [['narrative', narrative, softN], ['linkedin', posts, softP]]) {
    summaries[stage] = {};
    for (const arm of ['F', 'H']) {
        summaries[stage][arm] = {};
        for (const c of [...cases, 'all']) {
            const rows = Object.entries(scores).filter(([id]) => id.endsWith(arm) && (c === 'all' || id.includes(c + '-')));
            summaries[stage][arm][c] = {
                scored: rows.length,
                full: rows.filter(([, row]) => [...keys, 'fidelity'].every(k => row[k] === 2)).length,
                sufficientByDimension: Object.fromEntries([...keys, 'fidelity'].map(k => [k, rows.filter(([, row]) => row[k] === 2).length]))
            };
        }
    }
}
const pairs = config.narrative.filter(id => id.endsWith('F')).map(id => id.slice(0, -1)).filter(prefix =>
    ['F', 'H'].every(arm => posts['linkedin-' + prefix + arm]?.fidelity === 2));
const paired = {};
for (const arm of ['F', 'H']) {
    const postRows = pairs.map(p => posts['linkedin-' + p + arm]);
    const allRows = usage.filter(r => pairs.some(p => r.id === p + arm || r.id === 'linkedin-' + p + arm));
    paired[arm] = {
        appealEnding: postRows.reduce((sum, row) => sum + row.appeal + row.ending, 0),
        totalSoft: postRows.reduce((sum, row) => sum + softP.reduce((total, k) => total + row[k], 0), 0),
        tokens: allRows.reduce((sum, row) => sum + row.total, 0),
        usd: allRows.reduce((sum, row) => sum + row.usd, 0)
    };
}
const hNarrative = summaries.narrative.H;
const hPosts = summaries.linkedin.H;
const gates = {
    noUnsafeHandoff: Object.values(gate).every(value => value !== 'executed before fidelity correction'),
    allNarrativeCompleted: usage.filter(r => r.stage === 'narrative').length === config.narrative.length,
    allHFidelitySafe: hNarrative.all.sufficientByDimension.fidelity === 12,
    narrativeSoft: hNarrative.all.full >= 10 && cases.every(c => hNarrative[c].full >= 2),
    eligibleComposeCompleted: Object.entries(gate).every(([id, decision]) => decision !== 'eligible' || usage.some(r => r.id === id)),
    allHPostsFidelitySafe: hPosts.all.sufficientByDimension.fidelity === 12,
    composeSoft: hPosts.all.full >= 10 && cases.every(c => hPosts[c].full >= 2),
    pairedCoverage: pairs.length >= 6 && new Set(pairs.map(p => p.split('-')[0])).size >= 3,
    pairedQuality: paired.H.appealEnding > paired.F.appealEnding && paired.H.totalSoft >= paired.F.totalSoft,
    pairedTokens: pairs.length > 0 && paired.H.tokens <= 1.1 * paired.F.tokens,
    pairedCosts: pairs.length > 0 && paired.H.usd <= 1.1 * paired.F.usd
};
const decision = Object.values(gates).every(Boolean) ? 'GO' : 'NO-GO';
save('usage-costs.json', { ratesPerMillionUsd: rates, pricingBasis: 'Standard estimate, not invoice', source: 'https://developers.openai.com/api/docs/models/gpt-6-luna', usage });
save('decision.json', { decision, gates, summaries, gate, pairs, paired });
console.log(responses.size + ' unique responses verified. Decision: ' + decision);
