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
using Nueyon.Compose.Domain;

var root = @"C:\Workspaces\Nueyon.Compose";
var output = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
var baseline = Path.Combine(root,"PromptExperiments/Research/luna-baseline-2026-11-12");
var phase=args.Single();
if(phase!="calibrate" && phase!="candidate") throw new Exception("Unknown phase");
if(phase=="candidate" && !File.Exists(Path.Combine(output,"calibration-approved.md"))) throw new Exception("Calibration approval absent");
var secretsPath=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json");
using var secrets=JsonDocument.Parse(File.ReadAllText(secretsPath));
var secretRoot=secrets.RootElement;
var key=secretRoot.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():secretRoot.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
var json=new JsonSerializerOptions(JsonSerializerDefaults.Web){WriteIndented=true};
using var scope=PromptDirectory.Use(Path.Combine(output,"prompts"));
var conditions=phase=="calibrate"?new[]{"baseline-judge","control-judge"}:new[]{"candidate-research","candidate-judge"};
foreach(var condition in conditions)
foreach(var id in new[]{"intro","005","010"})
{
 var dir=Path.Combine(output,condition,id); Directory.CreateDirectory(dir);
 var marker=Path.Combine(dir,"attempt-started.json");
 if(File.Exists(marker)) throw new Exception("Refusing repeated call: "+condition+"/"+id);
 if(Directory.GetFiles(output,"attempt-started.json",SearchOption.AllDirectories).Length>=12) throw new Exception("Global call budget exhausted");
 var input=JsonSerializer.Deserialize<ResearchInput>(File.ReadAllText(Path.Combine(baseline,id,"research-input.json")),json)!;
 ResearchResult? research=null;
 if(condition!="candidate-research")
 {
  var resultPath=condition=="candidate-judge"?Path.Combine(output,"candidate-research",id,"research-result.json"):Path.Combine(baseline,id,"research-result.json");
  research=JsonSerializer.Deserialize<ResearchResult>(File.ReadAllText(resultPath),json)!;
  if(condition=="control-judge")
  {
   var content=research.Content;
   if(id=="intro") content=content.Replace("The source does not establish that juxtaposing the broad company ambition with Compose makes the ambition tangible; this is an editorial assessment in the Selected Idea.","The source describes Compose as the first concrete expression of the vision (Facts 9 and 13); it does not establish the effect of juxtaposing ambition and Compose on readers.");
   if(id=="005") content=content.Replace("### Unknowns","### Unknowns\n\n- Causal attribution is limited: the source Limitations says \"Idea and Research outputs vary between runs.\" Baselines were produced on 2026-11-03, candidates on 2026-11-04. Identical workflow inputs and other prompt files do not establish identical intermediate stage outputs or isolate the Narrative change.\n");
   if(id=="010") content=content.Replace("### Interpretations","- Experiment 009 Baseline 001: the first reported factual failure is Compose (\"repeatable process that reliably converts\"); selected Idea and earlier chain remain company-first and downstream hierarchy scores 2/2. This is a factual failure with preserved hierarchy, not a first hierarchy break at Compose.\n- The source recommends comparing actual Synthesis input before changing Narrative; this is a recommendation, not demonstrated intervention efficacy.\n\n### Interpretations");
   research=new ResearchResult(content);
  }
  File.WriteAllText(Path.Combine(dir,"research-result.json"),JsonSerializer.Serialize(research,json));
  File.WriteAllText(Path.Combine(dir,"synthesis-filter-output.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content));
 }
 var handler=new CaptureHandler(dir);
 using var http=new HttpClient(handler){Timeout=TimeSpan.FromMinutes(4)};
 var client=new OpenAIClient(new ApiKeyCredential(key!),new OpenAIClientOptions{RetryPolicy=new ClientRetryPolicy(0),Transport=new HttpClientPipelineTransport(http)});
 var instructions=condition=="candidate-research"?ResearchAgent.GetSystemInstructions():ResearchSemanticJudge.GetSystemInstructions();
 var ai=client.GetChatClient("gpt-6-luna").AsIChatClient().AsAIAgent(instructions,"OpenAIAgent");
 File.WriteAllText(marker,JsonSerializer.Serialize(new{id,condition,model="gpt-6-luna",started=DateTimeOffset.UtcNow,maxHttpRequests=1,retries=0},json));
 try {
  if(condition=="candidate-research")
  {
   var result=await new ResearchAgent(ai,NullLogger<ResearchAgent>.Instance).ExecuteAsync(new AgentExecutionContext(Guid.NewGuid()),input);
   File.WriteAllText(Path.Combine(dir,"research-result.json"),JsonSerializer.Serialize(result,json));
   File.WriteAllText(Path.Combine(dir,"research-content.md"),result.Content);
   File.WriteAllText(Path.Combine(dir,"synthesis-filter-output.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(result.Content));
  }
  else
  {
   var verdict=await new ResearchSemanticJudge(ai).EvaluateAsync(input,research!);
   File.WriteAllText(Path.Combine(dir,"verdict.json"),JsonSerializer.Serialize(verdict,json));
  }
  Console.WriteLine(condition+"/"+id+": success; HTTP requests="+handler.Count);
 } catch(Exception ex) {File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());Console.WriteLine(condition+"/"+id+": failed; "+ex.Message);}
}
class CaptureHandler(string dir):DelegatingHandler(new HttpClientHandler())
{
 public int Count {get;private set;}
 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct) {
  if(++Count>1) throw new Exception("HTTP request budget exceeded");
  File.WriteAllText(Path.Combine(dir,"request.json"),await request.Content!.ReadAsStringAsync(ct));
  var response=await base.SendAsync(request,ct);
  File.WriteAllText(Path.Combine(dir,"response-raw.json"),await response.Content.ReadAsStringAsync(ct));
  File.WriteAllText(Path.Combine(dir,"http-metadata.json"),JsonSerializer.Serialize(new{status=(int)response.StatusCode},new JsonSerializerOptions{WriteIndented=true}));
  return response;
 }
}
