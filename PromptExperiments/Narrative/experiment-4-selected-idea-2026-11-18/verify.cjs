const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const read = file => JSON.parse(fs.readFileSync(path.join(__dirname, file), 'utf8').replace(/^\uFEFF/, ''));
const { model, maxOutputTokens, schedule } = read('config.json');
assert.equal(schedule.length, 48);
assert.equal(new Set(schedule).size, 48);
const orders = {};
for (let offset = 0; offset < 36; offset += 3) {
    const block = schedule.slice(offset, offset + 3);
    assert.equal(new Set(block.map(id => id.slice(0, -1))).size, 1);
    const order = block.map(id => id.at(-1)).join('');
    assert.equal([...order].sort().join(''), 'ABC');
    orders[order] = (orders[order] ?? 0) + 1;
}
assert.equal(Object.keys(orders).length, 6);
assert(Object.values(orders).every(count => count === 2));
const seen = new Set();
const rows = [];
let linkedInSettings;
const narrativeSettings = new Map();

for (const id of schedule) {
    const file = name => read(`calls/${id}/${name}.json`);
    const request = file('request');
    const response = file('response-raw');
    const http = file('http');
    const attempt = file('attempt');
    const linkedin = id.startsWith('linkedin-');
    const narrativeId = linkedin ? id.slice('linkedin-'.length) : id;
    const { messages, ...settings } = request;
    assert.equal(attempt.run, id);
    assert.equal(attempt.model, model);
    assert.equal(request.model, model);
    assert.equal(response.model, model);
    assert.equal(http.status, 200);
    assert.equal(response.choices[0].finish_reason, 'stop');
    assert(response.id && !seen.has(response.id));
    seen.add(response.id);
    assert.deepEqual(messages.map(message => message.role), ['system', 'user']);
    if (linkedin) {
        assert.equal(settings.response_format.json_schema.name, 'ComposeResult');
        assert.equal(settings.max_completion_tokens, undefined);
        linkedInSettings ??= settings;
        assert.deepEqual(settings, linkedInSettings);
        const parent = read(`calls/${narrativeId}/result.json`);
        assert.deepEqual(file('input').narrative, { content: parent.content });
        assert(messages[1].content.includes(parent.content));
    } else {
        assert.equal(settings.response_format.json_schema.name, 'NarrativeResult');
        assert.equal(settings.max_completion_tokens, maxOutputTokens);
        const block = id.slice(0, -2);
        if (!narrativeSettings.has(block)) narrativeSettings.set(block, settings);
        assert.deepEqual(settings, narrativeSettings.get(block));
        assert.deepEqual(file('input'), read(`cases/${id.split('-')[0]}/input.json`));
    }
    const { prompt_tokens, completion_tokens, total_tokens, prompt_tokens_details, completion_tokens_details } = response.usage;
    assert.equal(total_tokens, prompt_tokens + completion_tokens);
    rows.push({ id, stage: linkedin ? 'LinkedIn' : 'Narrative', arm: id.at(-1), responseId: response.id,
        input: prompt_tokens, completion: completion_tokens, total: total_tokens,
        cached: prompt_tokens_details?.cached_tokens ?? null, reasoning: completion_tokens_details?.reasoning_tokens ?? null });
}

const totals = {};
for (const row of rows) {
    const bucket = totals[`${row.stage}-${row.arm}`] ??= { calls: 0, input: 0, completion: 0, total: 0 };
    bucket.calls++;
    for (const key of ['input', 'completion', 'total']) bucket[key] += row[key];
}
fs.writeFileSync(path.join(__dirname, 'usage.json'), JSON.stringify({ totals, rows }, null, 2) + '\n');
console.table(totals);
console.log('48 unique responses, provider usage, paired model settings, fixture and Compose input mappings verified.');
