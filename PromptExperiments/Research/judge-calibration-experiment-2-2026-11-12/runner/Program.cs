using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using OpenAI;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;

var output=Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,"../../../../"));
var json=new JsonSerializerOptions(JsonSerializerDefaults.Web){WriteIndented=true,PropertyNameCaseInsensitive=true};
using var freeze=JsonDocument.Parse(File.ReadAllText(Path.Combine(output,"freeze.json")));
var expectedHash=freeze.RootElement.GetProperty("promptSha256").GetString();
var actualHash=Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(Path.Combine(output,"prompts/Research/ResearchValidation.system.prompt.md")))).ToLowerInvariant();
if(actualHash!=expectedHash) throw new Exception("Frozen prompt changed");
if(Directory.GetFiles(output,"attempt-started.json",SearchOption.AllDirectories).Length!=0) throw new Exception("Refusing rerun of experiment");
using var secrets=JsonDocument.Parse(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json")));
var sr=secrets.RootElement;
var key=sr.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():sr.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
using var scope=PromptDirectory.Use(Path.Combine(output,"prompts"));
foreach(var c in freeze.RootElement.GetProperty("cases").EnumerateArray())
{
 var id=c.GetProperty("id").GetString()!;
 var dir=Path.Combine(output,"calls",id);Directory.CreateDirectory(dir);
 if(Directory.GetFiles(output,"attempt-started.json",SearchOption.AllDirectories).Length>=12) throw new Exception("Hard budget exhausted");
 var data=Path.Combine(output,"cases",id);
 var input=JsonSerializer.Deserialize<ResearchInput>(File.ReadAllText(Path.Combine(data,"input.json")),json)!;
 var research=JsonSerializer.Deserialize<ResearchResult>(File.ReadAllText(Path.Combine(data,"research-result.json")),json)!;
 File.WriteAllText(Path.Combine(dir,"synthesis-filter-output.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content));
 var handler=new CaptureHandler(dir);
 using var http=new HttpClient(handler){Timeout=TimeSpan.FromMinutes(4)};
 var client=new OpenAIClient(new ApiKeyCredential(key!),new OpenAIClientOptions{RetryPolicy=new ClientRetryPolicy(0),Transport=new HttpClientPipelineTransport(http)});
 var ai=client.GetChatClient("gpt-6-luna").AsIChatClient().AsAIAgent(ResearchSemanticJudge.GetSystemInstructions(),"OpenAIAgent");
 File.WriteAllText(Path.Combine(dir,"attempt-started.json"),JsonSerializer.Serialize(new{id,model="gpt-6-luna",started=DateTimeOffset.UtcNow,maxHttpRequests=1,retries=0,promptSha256=actualHash},json));
 try {
  var verdict=await new ResearchSemanticJudge(ai).EvaluateAsync(input,research);
  File.WriteAllText(Path.Combine(dir,"verdict.json"),JsonSerializer.Serialize(verdict,json));
  Console.WriteLine(id+": accepted="+verdict.Accepted+"; issues="+verdict.BlockingIssues.Count+"; requests="+handler.Count);
 }catch(Exception ex){File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());Console.WriteLine(id+": failed: "+ex.Message);}
}
class CaptureHandler(string dir):DelegatingHandler(new HttpClientHandler())
{
 public int Count{get;private set;}
 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct){
  if(++Count>1)throw new Exception("HTTP request budget exceeded");
  File.WriteAllText(Path.Combine(dir,"request.json"),await request.Content!.ReadAsStringAsync(ct));
  var response=await base.SendAsync(request,ct);
  File.WriteAllText(Path.Combine(dir,"response-raw.json"),await response.Content.ReadAsStringAsync(ct));
  File.WriteAllText(Path.Combine(dir,"http-metadata.json"),JsonSerializer.Serialize(new{status=(int)response.StatusCode,completed=DateTimeOffset.UtcNow}));
  return response;
 }
}
