const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const read = (file) => fs.readFileSync(path.join(__dirname, file), 'utf8').replace(/^\uFEFF/, '');
const json = (file) => JSON.parse(read(file));
const config = json('config.json');
const system = read('prompts/Narrative/Narrative.system.prompt.md');
const user = read('prompts/Narrative/Narrative.user.prompt.md');

const checks = config.schedule.map((id) => {
    const dir = 'calls/' + id;
    const fixture = 'cases/' + id.split('-')[0];
    const input = json(dir + '/input.json');
    assert.equal(read(dir + '/input.json'), read(fixture + '/input.json'));
    assert.deepEqual(input.selectedIdea, json(fixture + '/idea.json'));
    assert.equal(input.synthesis.content, json(fixture + '/synthesis-result.json').content);
    const idea = input.selectedIdea;
    const fields = {
        IdeaTitle: idea.title ?? idea.Title,
        IdeaDescription: idea.description ?? idea.Description,
        IdeaAudience: idea.audience ?? idea.Audience,
        IdeaRationale: idea.rationale ?? idea.Rationale,
        IdeaEvidence: idea.evidence ?? idea.Evidence,
        SynthesisContent: input.synthesis.content,
    };
    const request = json(dir + '/request.json');
    assert.deepEqual(request.messages.map(message => [message.role, message.content]), [
        ['system', system],
        ['user', user.replace(/\{\{(\w+)\}\}/g, (_, name) => fields[name])],
    ]);
    assert.equal(request.model, config.model);
    assert.equal(request.max_completion_tokens, config.maxOutputTokens);
    assert.deepEqual(Object.keys(request).sort(), ['max_completion_tokens', 'messages', 'model', 'response_format']);
    assert.equal(request.response_format.type, 'json_schema');
    assert.equal(request.response_format.json_schema.name, 'NarrativeResult');
    assert.equal(json(dir + '/http.json').status, 200);
    const response = json(dir + '/response-raw.json');
    assert.equal(response.model, config.model);
    assert.equal(response.choices[0].finish_reason, 'stop');
    const parsed = JSON.parse(response.choices[0].message.content);
    const result = json(dir + '/result.json');
    assert.equal(parsed.content ?? parsed.Content, result.content);
    assert.equal(result.content, read(dir + '/content.md'));
    const usage = response.usage;
    assert.equal(usage.total_tokens, usage.prompt_tokens + usage.completion_tokens);
    return { id, responseId: response.id, model: response.model, usage };
});
assert.equal(new Set(checks.map(check => check.responseId)).size, config.schedule.length);
const total = Object.fromEntries(['prompt_tokens', 'completion_tokens', 'total_tokens']
    .map(key => [key, checks.reduce((sum, check) => sum + check.usage[key], 0)]));
fs.writeFileSync(path.join(__dirname, 'usage.json'), JSON.stringify({ total, runs: checks }, null, 2) + '\n');
console.log('Verified requests, fixture mappings, distinct responses, results and usage:', checks.length);

