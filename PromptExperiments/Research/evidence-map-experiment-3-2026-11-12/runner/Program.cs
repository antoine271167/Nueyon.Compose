using System.ClientModel;
using System.ClientModel.Primitives;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using OpenAI;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Domain;

var output=Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,"../../../../"));
var root=Path.GetFullPath(Path.Combine(output,"../../.."));
var json=new JsonSerializerOptions(JsonSerializerDefaults.Web){WriteIndented=true,PropertyNameCaseInsensitive=true,AllowDuplicateProperties=false,UnmappedMemberHandling=JsonUnmappedMemberHandling.Disallow};
var mode=args.Single();
using var freeze=JsonDocument.Parse(File.ReadAllText(Path.Combine(output,"freeze.json")));
foreach(var f in freeze.RootElement.GetProperty("files").EnumerateObject())
 if(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,f.Name)))).ToLowerInvariant()!=f.Value.GetString())throw new Exception("Frozen file changed: "+f.Name);
var pairs=freeze.RootElement.GetProperty("pairs").EnumerateArray().Select(x=>x.GetProperty("id").GetString()!).ToArray();
if(mode=="self-check"){

 var input=Load<ResearchInput>("cases/intro/bad/input.json");
 var research=Load<ResearchResult>("cases/intro/bad/research-result.json");
 foreach(var response in new[]{"{\"accepted\":true,\"blockingIssues\":[]}","{\"accepted\":true,\"accepted\":false,\"blockingIssues\":[]}","{\"accepted\":true,\"blockingIssues\":[],\"extra\":1}","{\"accepted\":false,\"blockingIssues\":[{\"category\":\"sourceFidelity\",\"researchLocation\":\"Facts\",\"problem\":\"x\",\"sourceQuote\":\"not in original source\",\"constraint\":\"x\"}]}"}) {
  bool ok=true;try{await new ResearchSemanticJudge(new ReplayClient(response).AsAIAgent("offline")).EvaluateAsync(input,research);}catch(InvalidOperationException){ok=false;}
  if(ok!=response.StartsWith("{\"accepted\":true,\"blockingIssues\":[]}"))throw new Exception("Parser self-check failed");
 }
 var invalid=new EvidenceMap{Items=[new(){Id="x",Kind="claim",Proposition="x",SourceQuotes=["not a quote"],Materiality="x",Boundary="x"}],IdeaChecks=[],Limitations=[]};
 if(ValidateMap(invalid,input.Input.Content).Count==0)throw new Exception("Anchor self-check failed");
 Console.WriteLine("Offline production-parser and mapper-anchor self-checks passed.");return;
}
if(mode is not ("maps" or "judges"))throw new Exception("Use maps/judges/self-check");
using var runLock=new FileStream(Path.Combine(output,mode+".lock"),FileMode.CreateNew,FileAccess.Write,FileShare.None);
if(mode=="judges"&&!File.Exists(Path.Combine(output,"map-audit.json")))throw new Exception("Map audit must exist before judges");
using var secrets=JsonDocument.Parse(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json")));
var sr=secrets.RootElement;
var key=sr.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():sr.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
foreach(var id in pairs){
 foreach(var variant in mode=="maps"?new[]{"map"}:new[]{"bad","correct"}){
  var callId=id+"-"+variant;
  var dir=Path.Combine(output,"calls",callId);Directory.CreateDirectory(dir);
  if(Directory.GetFiles(output,"attempt-started.json",SearchOption.AllDirectories).Length>=18)throw new Exception("Hard budget exhausted");
  var input=Load<ResearchInput>($"cases/{id}/{(variant=="map"?"bad":variant)}/input.json");
  var handler=new CaptureHandler(dir);
  using var http=new HttpClient(handler){Timeout=TimeSpan.FromMinutes(4)};
  var client=new OpenAIClient(new ApiKeyCredential(key!),new OpenAIClientOptions{RetryPolicy=new ClientRetryPolicy(0),Transport=new HttpClientPipelineTransport(http)});
  using var marker=new FileStream(Path.Combine(dir,"attempt-started.json"),FileMode.CreateNew);
  JsonSerializer.Serialize(marker,new{callId,model="gpt-6-luna",started=DateTimeOffset.UtcNow,maxHttpRequests=1,retries=0},json);marker.Flush();
  try{
   if(mode=="maps"){
    var ai=client.GetChatClient("gpt-6-luna").AsIChatClient().AsAIAgent(File.ReadAllText(Path.Combine(output,"prompts/mapper.md")),"Mapper");
    var payload=JsonSerializer.Serialize(new{originalSource=input.Input.Content,selectedIdea=input.SelectedIdea.Idea},json);
    var response=await ai.RunAsync(payload,null,new ChatClientAgentRunOptions(new ChatOptions{ResponseFormat=ChatResponseFormat.ForJsonSchema<EvidenceMap>(json,nameof(EvidenceMap))}));
    File.WriteAllText(Path.Combine(dir,"map-raw.json"),response.Text);
    var map=JsonSerializer.Deserialize<EvidenceMap>(response.Text,json)??throw new Exception("Null map");
    Save(dir,"map.json",map);Save(dir,"anchor-validation.json",ValidateMap(map,input.Input.Content));
    Console.WriteLine(callId+": items="+map.Items.Count+", anchor/contract errors="+ValidateMap(map,input.Input.Content).Count);
   }else{
    var research=Load<ResearchResult>($"cases/{id}/{variant}/research-result.json");
    var mapText=File.ReadAllText(Path.Combine(output,"calls",id+"-map","map-raw.json"));
    File.WriteAllText(Path.Combine(dir,"synthesis-filter-output.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content));
    // Wrapper adds a separate user-payload field. The unchanged production judge
    // generates source/Idea/Research/filter fields and validates the answer.
    var wrapped=new MapClient(client.GetChatClient("gpt-6-luna").AsIChatClient(),mapText);
    var ai=wrapped.AsAIAgent(File.ReadAllText(Path.Combine(output,"prompts/judge.md")),"Judge");
    var verdict=await new ResearchSemanticJudge(ai).EvaluateAsync(input,research);
    Save(dir,"verdict.json",verdict);
    Console.WriteLine(callId+": accepted="+verdict.Accepted+", issues="+verdict.BlockingIssues.Count);
   }
  }catch(Exception ex){File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());Console.WriteLine(callId+": FAILED "+ex.Message);}
 }
}
T Load<T>(string path)=>JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(output,path)),json)!;
void Save(string dir,string name,object value)=>File.WriteAllText(Path.Combine(dir,name),JsonSerializer.Serialize(value,json));
static List<string> ValidateMap(EvidenceMap map,string source){
 var errors=new List<string>();var ids=new HashSet<string>();
 foreach(var i in map.Items){
  if(string.IsNullOrWhiteSpace(i.Id)||!ids.Add(i.Id))errors.Add("Duplicate/empty id");
  if(!new[]{"claim","explicitMeaning","causalLimit","counterexample","negativeControl"}.Contains(i.Kind))errors.Add(i.Id+": invalid kind");
  if(new[]{i.Proposition,i.Materiality,i.Boundary}.Any(string.IsNullOrWhiteSpace))errors.Add(i.Id+": incomplete fields");
  if(i.SourceQuotes.Count==0||i.SourceQuotes.Any(q=>string.IsNullOrWhiteSpace(q)||!source.Contains(q,StringComparison.Ordinal)))errors.Add(i.Id+": invalid source anchor");
 }
 foreach(var c in map.IdeaChecks)if(!new[]{"supported","qualified","unsupported"}.Contains(c.Status)||c.SourceItemIds.Any(id=>!ids.Contains(id)))errors.Add("Invalid idea check");
 if(map.Items.Count==0)errors.Add("Empty map");return errors;
}
sealed class MapClient(IChatClient inner,string map):DelegatingChatClient(inner){
 public override Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default){
  var updated=messages.Select(m=>{
   if(m.Role!=ChatRole.User)return m;
   var node=JsonNode.Parse(m.Text!)!.AsObject();
   node["originalSource"]=node["source"]!.DeepClone();node.Remove("source");
   try{node["evidenceMap"]=JsonNode.Parse(map);}catch(JsonException){node["evidenceMapInvalidRaw"]=map;}
   return new ChatMessage(ChatRole.User,node.ToJsonString());
  }).ToArray();return base.GetResponseAsync(updated,options,cancellationToken);
 }
}
sealed class ReplayClient(string text):IChatClient{
 public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default)=>Task.FromResult(new ChatResponse([new ChatMessage(ChatRole.Assistant,text)]));
 public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default)=>throw new NotSupportedException();
 public object? GetService(Type type,object? key=null)=>null;
 public void Dispose(){}
}
sealed class CaptureHandler(string dir):DelegatingHandler(new HttpClientHandler()){
 int count;
 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct){
  if(Interlocked.Increment(ref count)>1)throw new Exception("Per-attempt HTTP cap exceeded");
  File.WriteAllText(Path.Combine(dir,"request.json"),await request.Content!.ReadAsStringAsync(ct));
  var response=await base.SendAsync(request,ct);
  File.WriteAllText(Path.Combine(dir,"response-raw.json"),await response.Content.ReadAsStringAsync(ct));
  File.WriteAllText(Path.Combine(dir,"http-metadata.json"),JsonSerializer.Serialize(new{status=(int)response.StatusCode,completed=DateTimeOffset.UtcNow}));return response;
 }
}
sealed record EvidenceMap{
 public required List<EvidenceItem> Items{get;init;}
 public required List<IdeaCheck> IdeaChecks{get;init;}
 public required List<string> Limitations{get;init;}
}
sealed record EvidenceItem{
 public required string Id{get;init;}
 public required string Kind{get;init;}
 public required string Proposition{get;init;}
 public required List<string> SourceQuotes{get;init;}
 public required string Materiality{get;init;}
 public required string Boundary{get;init;}
}
sealed record IdeaCheck{
 public required string Proposition{get;init;}
 public required List<string> SourceItemIds{get;init;}
 public required string Status{get;init;}
}
