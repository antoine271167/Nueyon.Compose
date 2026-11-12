using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Application.Workflows;
using Nueyon.Compose.Domain;

var root = @"C:\Workspaces\Nueyon.Compose";
var output = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
var frozen = Path.Combine(root,"PromptExperiments/IdeaGenerator/luna-simplified-comparison-2026-11-12");
var sources = Path.Combine(root,"PromptExperiments/IdeaGenerator/legacy-simplified-acceptance-2026-11-12");
var secretsPath=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json");
using var secrets=JsonDocument.Parse(File.ReadAllText(secretsPath));
var secretRoot=secrets.RootElement;
var key=secretRoot.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():secretRoot.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
var json=new JsonSerializerOptions{PropertyNameCaseInsensitive=true,WriteIndented=true};
using var scope=PromptDirectory.Use(Path.Combine(root,"src/Nueyon.Compose.Application/Prompts"));
foreach(var id in new[]{"intro","005","010"})
{
    var dir=Path.Combine(output,id); Directory.CreateDirectory(dir);
    var marker=Path.Combine(dir,"attempt-started.json");
    if(File.Exists(marker)) throw new Exception("Refusing repeated call: "+id);
    var sourcePath=Path.Combine(sources,id+"-input.json");
    var ideaPath=Path.Combine(frozen,id+"-selected.json");
    File.Copy(sourcePath,Path.Combine(dir,"source-input.json"),false);
    File.Copy(ideaPath,Path.Combine(dir,"idea-input.json"),false);
    var source=JsonSerializer.Deserialize<StoryInput>(File.ReadAllText(sourcePath),json)!;
    var idea=JsonSerializer.Deserialize<Idea>(File.ReadAllText(ideaPath),json)!;
    var input=new ResearchInput(source,new SelectedIdea(idea));
    File.WriteAllText(Path.Combine(dir,"research-input.json"),JsonSerializer.Serialize(input,json));
    File.WriteAllText(Path.Combine(dir,"source.md"),source.Content);
    var handler=new CaptureHandler(dir);
    using var http=new HttpClient(handler){Timeout=TimeSpan.FromMinutes(4)};
    var client=new OpenAIClient(new ApiKeyCredential(key!),new OpenAIClientOptions{
        RetryPolicy=new ClientRetryPolicy(0),Transport=new HttpClientPipelineTransport(http)});
    var ai=client.GetChatClient("gpt-6-luna").AsIChatClient().AsAIAgent(ResearchAgent.GetSystemInstructions(),"OpenAIAgent");
    var agent=new ResearchAgent(ai,NullLogger<ResearchAgent>.Instance);
    var started=DateTimeOffset.UtcNow;
    File.WriteAllText(marker,JsonSerializer.Serialize(new{id,model="gpt-6-luna",started,maxHttpRequests=1,retries=0},json));
    try {
        var result=await agent.ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()),input);
        File.WriteAllText(Path.Combine(dir,"research-result.json"),JsonSerializer.Serialize(result,json));
        File.WriteAllText(Path.Combine(dir,"research-content.md"),result.Content);
        File.WriteAllText(Path.Combine(dir,"synthesis-filter-output.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(result.Content));
        Console.WriteLine(id+": success; HTTP requests="+handler.Count);
    } catch(Exception ex) {
        File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());
        Console.WriteLine(id+": failed; HTTP requests="+handler.Count);
    }
    File.WriteAllText(Path.Combine(dir,"metadata.json"),JsonSerializer.Serialize(new{id,model="gpt-6-luna",started,ended=DateTimeOffset.UtcNow,httpRequests=handler.Count,retries=0},json));
}
class CaptureHandler(string dir):DelegatingHandler(new HttpClientHandler())
{
    public int Count {get;private set;}
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct) {
        if(++Count>1) throw new Exception("HTTP request budget exceeded");
        File.WriteAllText(Path.Combine(dir,"request.json"),await request.Content!.ReadAsStringAsync(ct));
        var response=await base.SendAsync(request,ct);
        File.WriteAllText(Path.Combine(dir,"response-raw.json"),await response.Content.ReadAsStringAsync(ct));
        File.WriteAllText(Path.Combine(dir,"http-metadata.json"),JsonSerializer.Serialize(new{status=(int)response.StatusCode,headers=response.Headers.ToDictionary(h=>h.Key,h=>h.Value.ToArray())},new JsonSerializerOptions{WriteIndented=true}));
        return response;
    }
}
