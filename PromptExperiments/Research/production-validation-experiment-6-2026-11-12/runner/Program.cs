using System.ClientModel;
using System.ClientModel.Primitives;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Application.Agents.Narrative;
using Nueyon.Compose.Application.Agents.Compose;
using Nueyon.Compose.Application.Prompts;
using Nueyon.Compose.Domain;

var output=Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,"../../../../"));
var root=Path.GetFullPath(Path.Combine(output,"../../.."));
var json=new JsonSerializerOptions(JsonSerializerDefaults.Web){WriteIndented=true,PropertyNameCaseInsensitive=true};
var mode=args.Single();
VerifyFreeze();
if(mode=="replay"){
 var checks=new List<object>();
 foreach(var dir in Directory.GetDirectories(Path.Combine(output,"calls")).Order()){
  if(!File.Exists(Path.Combine(dir,"response-raw.json")))continue;
  using var raw=JsonDocument.Parse(File.ReadAllText(Path.Combine(dir,"response-raw.json")));
  if(!raw.RootElement.TryGetProperty("choices",out var choices))continue;
  var text=choices[0].GetProperty("message").GetProperty("content").GetString()!;
  var stage=JsonDocument.Parse(File.ReadAllText(Path.Combine(dir,"attempt-started.json"))).RootElement.GetProperty("stage").GetString()!;
  if(stage=="evaluation"){using var parsed=JsonDocument.Parse(text);checks.Add(new{id=Path.GetFileName(dir),stage,validJson=true});continue;}
  var input=File.ReadAllText(Path.Combine(dir,"input.json"));
  var client=new ReplayClient(text);
  var context=new AgentExecutionContext(Guid.NewGuid());
  object result=stage switch{
   "Research"=>await new ResearchAgent(client.AsAIAgent("offline"),NullLogger<ResearchAgent>.Instance).ExecuteAsync(context,JsonSerializer.Deserialize<ResearchInput>(input,json)!),
   "Synthesis"=>await new SynthesizerAgent(client.AsAIAgent("offline"),NullLogger<SynthesizerAgent>.Instance).ExecuteAsync(context,JsonSerializer.Deserialize<SynthesisInput>(input,json)!),
   "Narrative"=>await new NarrativeAgent(client.AsAIAgent("offline"),NullLogger<NarrativeAgent>.Instance).ExecuteAsync(context,JsonSerializer.Deserialize<NarrativeInput>(input,json)!),
   "Compose"=>await new ComposeAgent(client.AsAIAgent("offline"),NullLogger<ComposeAgent>.Instance).ExecuteAsync(context,JsonSerializer.Deserialize<ComposeInput>(input,json)!),
   _=>throw new Exception("Stage")};
  if(JsonSerializer.Serialize(result,json)!=File.ReadAllText(Path.Combine(dir,"result.json")))throw new Exception("Replay mismatch "+dir);
  if(result is ResearchResult rr && File.ReadAllText(Path.Combine(dir,"projection.md"))!=SynthesisResearchFilter.ExtractFactsAndUnknowns(rr.Content))throw new Exception("Filter mismatch");
  checks.Add(new{id=Path.GetFileName(dir),stage,parsedAndExact=true});
 }
 Save(output,"verification-offline-replay.json",checks);Console.WriteLine("Offline production-agent replay: "+checks.Count);return;
}
if(mode!="run"&&mode!="evaluate")throw new Exception("run/evaluate/replay");
using var runLock=new FileStream(Path.Combine(output,mode+".lock"),FileMode.CreateNew,FileAccess.Write,FileShare.None);
using var secrets=JsonDocument.Parse(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json")));
var sr=secrets.RootElement;var key=sr.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():sr.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
using var plan=JsonDocument.Parse(File.ReadAllText(Path.Combine(output,"plan.json")));
if(mode=="run"){
 foreach(var lane in plan.RootElement.GetProperty("schedule").EnumerateArray()){
  var id=lane.GetProperty("id").GetString()!;var caseId=lane.GetProperty("case").GetString()!;var arm=lane.GetProperty("arm").GetString()!;
  using var promptScope=PromptDirectory.Use(Path.Combine(output,"prompts",arm));
  var input=Load<ResearchInput>($"cases/{caseId}/input.json");var idea=input.SelectedIdea.Idea;
  try{
   var r=await Call<ResearchResult>(id,"Research",input,ResearchAgent.GetSystemInstructions(),async(ai)=>await new ResearchAgent(ai,NullLogger<ResearchAgent>.Instance).ExecuteAsync(Context(),input));
   CheckResearch(r);
   var projection=SynthesisResearchFilter.ExtractFactsAndUnknowns(r.Content);File.WriteAllText(Path.Combine(output,"calls",id+"-Research","projection.md"),projection);
   var si=new SynthesisInput(idea,new ResearchForSynthesis(projection));
   var s=await Call<SynthesisResult>(id,"Synthesis",si,SynthesizerAgent.GetSystemInstructions(),async(ai)=>await new SynthesizerAgent(ai,NullLogger<SynthesizerAgent>.Instance).ExecuteAsync(Context(),si));
   var ni=new NarrativeInput(idea,new SynthesisForNarrative(s.Content));
   var n=await Call<NarrativeResult>(id,"Narrative",ni,NarrativeAgent.GetSystemInstructions(),async(ai)=>await new NarrativeAgent(ai,NullLogger<NarrativeAgent>.Instance).ExecuteAsync(Context(),ni));
   var ci=new ComposeInput(new NarrativeForCompose(n.Content),new CompositionSpec(ContentFormat.LinkedInPost));
   await Call<ComposeResult>(id,"Compose",ci,ComposeAgent.GetSystemInstructions(),async(ai)=>await new ComposeAgent(ai,NullLogger<ComposeAgent>.Instance).ExecuteAsync(Context(),ci));
  }catch(BudgetException){throw;}catch(Exception ex){Save(output,id+"-pipeline-failure.json",new{id,error=ex.ToString()});Console.WriteLine(id+": pipeline failed; no retries or substitutions");}
 }
}else{
 // Packets created by a separate blinded-export step; no variant key enters a request.
 foreach(var packet in Directory.GetFiles(Path.Combine(output,"blind/packets"),"*.json").Order()){
  var id=Path.GetFileNameWithoutExtension(packet);var dir=Start(id,"evaluation");
  using var http=new HttpClient(new CaptureHandler(dir)){Timeout=TimeSpan.FromMinutes(5)};
  var client=NewClient(http);var capped=new CappedClient(client.GetChatClient("gpt-6-luna").AsIChatClient());
  try{
   var result=await capped.GetResponseAsync([new ChatMessage(ChatRole.System,File.ReadAllText(Path.Combine(output,"evaluation-system.md"))),new ChatMessage(ChatRole.User,File.ReadAllText(packet))],new ChatOptions{ResponseFormat=ChatResponseFormat.Json});
   File.WriteAllText(Path.Combine(dir,"evaluation.json"),result.Text);using var valid=JsonDocument.Parse(result.Text);Console.WriteLine(id+": evaluated");
  }catch(BudgetException){throw;}catch(Exception ex){File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());Console.WriteLine(id+": evaluation failed");}
 }
}
VerifyFreeze();Console.WriteLine("Completed "+mode+"; total attempts="+Directory.GetFiles(Path.Combine(output,"calls"),"attempt-started.json",SearchOption.AllDirectories).Length);
async Task<T> Call<T>(string id,string stage,object input,string system,Func<AIAgent,Task<T>> run){
 var dir=Start(id,stage);Save(dir,"input.json",input);File.WriteAllText(Path.Combine(dir,"system.md"),system);
 using var http=new HttpClient(new CaptureHandler(dir)){Timeout=TimeSpan.FromMinutes(5)};
 var ai=new CappedClient(NewClient(http).GetChatClient("gpt-6-luna").AsIChatClient()).AsAIAgent(system,stage);
 try{var result=await run(ai);Save(dir,"result.json",result!);var content=(string)typeof(T).GetProperty("Content")!.GetValue(result)!;File.WriteAllText(Path.Combine(dir,"content.md"),content);Console.WriteLine(id+"-"+stage+": complete");return result;}
 catch(Exception ex){File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());throw;}
}
OpenAIClient NewClient(HttpClient http)=>new(new ApiKeyCredential(key!),new OpenAIClientOptions{RetryPolicy=new ClientRetryPolicy(0),Transport=new HttpClientPipelineTransport(http)});
string Start(string id,string stage){VerifyFreeze();var all=Directory.Exists(Path.Combine(output,"calls"))?Directory.GetFiles(Path.Combine(output,"calls"),"attempt-started.json",SearchOption.AllDirectories).Length:0;if(all>=54)throw new BudgetException("Global 54-attempt cap");var dir=Path.Combine(output,"calls",id+"-"+stage);Directory.CreateDirectory(dir);using var marker=new FileStream(Path.Combine(dir,"attempt-started.json"),FileMode.CreateNew);JsonSerializer.Serialize(marker,new{id,stage,started=DateTimeOffset.UtcNow,model="gpt-6-luna",retries=0,maxHttpRequests=1,maxOutputTokens=6000},json);return dir;}
void VerifyFreeze(){using var freeze=JsonDocument.Parse(File.ReadAllText(Path.Combine(output,"freeze.json")));foreach(var f in freeze.RootElement.GetProperty("files").EnumerateObject())if(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,f.Name)))).ToLowerInvariant()!=f.Value.GetString())throw new Exception("Freeze mismatch: "+f.Name);}
T Load<T>(string p)=>JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(output,p)),json)!;
void Save(string dir,string name,object value)=>File.WriteAllText(Path.Combine(dir,name),JsonSerializer.Serialize(value,json));
static AgentExecutionContext Context()=>new(Guid.NewGuid());
static void CheckResearch(ResearchResult r){string[] expected=["### Facts","### Interpretations","### Unknowns","### Development sequence","### Editorial relevance"];if(!r.Content.Replace("\r\n","\n").Split('\n').Where(l=>l.StartsWith("###")).SequenceEqual(expected))throw new InvalidOperationException("Five exact headings required");if(!SynthesisResearchFilter.ExtractFactsAndUnknowns(r.Content).Contains("### Unknowns"))throw new InvalidOperationException("Incomplete projection");}
sealed class CappedClient(IChatClient inner):DelegatingChatClient(inner){public override Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default){options??=new();options.MaxOutputTokens=6000;return base.GetResponseAsync(messages,options,cancellationToken);}}
sealed class ReplayClient(string text):IChatClient{public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default)=>Task.FromResult(new ChatResponse([new ChatMessage(ChatRole.Assistant,text)]));public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default)=>throw new NotSupportedException();public object? GetService(Type type,object? key=null)=>null;public void Dispose(){}}
sealed class CaptureHandler(string dir):DelegatingHandler(new HttpClientHandler()){
 int count;
 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken ct){
  if(Interlocked.Increment(ref count)>1)throw new BudgetException("One HTTP request per attempt");
  var body=await request.Content!.ReadAsStringAsync(ct);File.WriteAllText(Path.Combine(dir,"request.json"),body);
  if(System.Text.Encoding.UTF8.GetByteCount(body)>180000)throw new BudgetException("Request byte cap");
  var response=await base.SendAsync(request,ct);var text=await response.Content.ReadAsStringAsync(ct);File.WriteAllText(Path.Combine(dir,"response-raw.json"),text);File.WriteAllText(Path.Combine(dir,"http-metadata.json"),JsonSerializer.Serialize(new{status=(int)response.StatusCode,completed=DateTimeOffset.UtcNow,httpRequests=count}));
  if(response.IsSuccessStatusCode){using var raw=JsonDocument.Parse(text);if(raw.RootElement.GetProperty("model").GetString()!="gpt-6-luna"||raw.RootElement.GetProperty("choices")[0].GetProperty("finish_reason").GetString()!="stop")throw new InvalidOperationException("Unexpected model or truncation");}
  return response;
 }
}
sealed class BudgetException(string message):Exception(message);
