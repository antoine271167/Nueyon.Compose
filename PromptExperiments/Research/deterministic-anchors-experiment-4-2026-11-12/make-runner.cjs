const fs=require('fs'),path=require('path');const out=__dirname;
fs.mkdirSync(path.join(out,'runner'),{recursive:true});
fs.copyFileSync(path.join(out,'../evidence-map-experiment-3-2026-11-12/runner/Runner.csproj'),path.join(out,'runner/Runner.csproj'));
let old=fs.readFileSync(path.join(out,'../evidence-map-experiment-3-2026-11-12/runner/Program.cs'),'utf8');
let imports=old.slice(0,old.indexOf('var output='));
let classes=old.slice(old.indexOf('sealed class ReplayClient'));
classes=classes.replace('SourceQuotes','SourceFragmentIds');
const main=`var output=Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,"../../../../"));
var root=Path.GetFullPath(Path.Combine(output,"../../.."));
var json=new JsonSerializerOptions(JsonSerializerDefaults.Web){WriteIndented=true,PropertyNameCaseInsensitive=true,AllowDuplicateProperties=false,UnmappedMemberHandling=JsonUnmappedMemberHandling.Disallow};
var mode=args.Single();
if(mode=="self-check"){
 foreach(var id in new[]{"intro","005","010"})CheckCatalog(Load<ResearchInput>($"cases/{id}/bad/input.json").Input.Content,Load<FragmentCatalog>($"fragments/{id}.json"));
 var source="lower **bold**\\r\\nNuëyon e\\u0301  \\n";
 var catalog=new FragmentCatalog(Hash(source),"UTF-16 code units",[new("F0001",0,source.Length,source)]);
 CheckCatalog(source,catalog);
 foreach(var bad in new[]{source.ToUpperInvariant(),source.Normalize(),source.Replace("\\r\\n","\\n"),source.TrimEnd()}){
  bool failed=false;try{CheckCatalog(bad,catalog);}catch{failed=true;}if(!failed)throw new Exception("Silent normalization");
 }
 foreach(var ids in new[]{new List<string>{"F9999"},new List<string>{"F0001","F0001"},new List<string>()}){
  var map=new EvidenceMap{Items=[new(){Id="S1",Kind="claim",Proposition="test",SourceFragmentIds=ids,Materiality="test",Boundary="test"}],IdeaChecks=[],Limitations=[]};
  if(ValidateMap(map,catalog).Count==0)throw new Exception("Bad IDs accepted");
 }
 bool mutated=false;try{CheckCatalog(source,catalog with { Fragments=[new("F0001",1,source.Length,source)] });}catch{mutated=true;}if(!mutated)throw new Exception("Bad offset accepted");
 var input=Load<ResearchInput>("cases/intro/bad/input.json");var research=Load<ResearchResult>("cases/intro/bad/research-result.json");
 foreach(var response in new[]{"{\\"accepted\\":true,\\"blockingIssues\\":[]}","{\\"accepted\\":false,\\"blockingIssues\\":[null]}","{\\"accepted\\":true,\\"accepted\\":false,\\"blockingIssues\\":[]}","{\\"accepted\\":true,\\"blockingIssues\\":[],\\"extra\\":1}"}){
  bool ok=true;try{await new ResearchSemanticJudge(new ReplayClient(response).AsAIAgent("offline")).EvaluateAsync(input,research);}catch(InvalidOperationException){ok=false;}
  if(ok!=response.StartsWith("{\\"accepted\\":true,\\"blockingIssues\\":[]}"))throw new Exception("Parser self-check failed");
 }
 Console.WriteLine("Offline span, ID, normalization and production-parser checks passed.");return;
}
using var freeze=JsonDocument.Parse(File.ReadAllText(Path.Combine(output,"freeze.json")));
foreach(var f in freeze.RootElement.GetProperty("files").EnumerateObject())
 if(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,f.Name)))).ToLowerInvariant()!=f.Value.GetString())throw new Exception("Frozen file changed: "+f.Name);
if(mode=="replay"){
 var checks=new List<object>();
 foreach(var call in Load<List<CallPlan>>("plan.json").Where(c=>c.Arm!="map")){
  var dir=Path.Combine(output,"calls",call.Id);var rawPath=Path.Combine(dir,"response-raw.json");if(!File.Exists(rawPath))continue;
  using var raw=JsonDocument.Parse(File.ReadAllText(rawPath));var text=raw.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()!;
  var input=Load<ResearchInput>($"cases/{call.Pair}/{call.Variant}/input.json");var research=Load<ResearchResult>($"cases/{call.Pair}/{call.Variant}/research-result.json");
  bool valid=true;try{var v=await new ResearchSemanticJudge(new ReplayClient(text).AsAIAgent("offline")).EvaluateAsync(input,research);if(JsonSerializer.Serialize(v,json)!=File.ReadAllText(Path.Combine(dir,"verdict.json")))throw new Exception("Replay mismatch");}catch(InvalidOperationException){valid=false;if(File.Exists(Path.Combine(dir,"verdict.json")))throw;}
  if(File.ReadAllText(Path.Combine(dir,"synthesis-filter-output.md"))!=SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content))throw new Exception("Filter mismatch");
  checks.Add(new{call.Id,parserValid=valid,filterMatches=true});
 }
 Save(output,"verification-offline-replay.json",checks);return;
}
if(mode is not ("maps" or "judges"))throw new Exception("Use maps/judges/self-check/replay");
using var runLock=new FileStream(Path.Combine(output,mode+".lock"),FileMode.CreateNew,FileAccess.Write,FileShare.None);
if(mode=="judges"&&!File.Exists(Path.Combine(output,"map-audit.json")))throw new Exception("Pre-judge map audit missing");
using var secrets=JsonDocument.Parse(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json")));
var sr=secrets.RootElement;var key=sr.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():sr.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
foreach(var call in Load<List<CallPlan>>("plan.json").Where(c=>mode=="maps"?c.Arm=="map":c.Arm!="map")){
 var dir=Path.Combine(output,"calls",call.Id);Directory.CreateDirectory(dir);
 if(call.Arm=="with"&&!File.Exists(Path.Combine(output,"calls",call.Pair+"-map","map.json"))){Save(dir,"skipped.json",new{reason="Invalid/missing map; counts as failed with-map result"});continue;}
 if(Directory.GetFiles(output,"attempt-started.json",SearchOption.AllDirectories).Length>=15)throw new Exception("Planned hard budget exhausted (absolute ceiling 18)");
 var input=Load<ResearchInput>($"cases/{call.Pair}/{(call.Arm=="map"?"bad":call.Variant)}/input.json");
 var handler=new CaptureHandler(dir);using var http=new HttpClient(handler){Timeout=TimeSpan.FromMinutes(4)};
 var client=new OpenAIClient(new ApiKeyCredential(key!),new OpenAIClientOptions{RetryPolicy=new ClientRetryPolicy(0),Transport=new HttpClientPipelineTransport(http)});
 using var marker=new FileStream(Path.Combine(dir,"attempt-started.json"),FileMode.CreateNew);
 JsonSerializer.Serialize(marker,new{call.Id,model="gpt-6-luna",started=DateTimeOffset.UtcNow,maxHttpRequests=1,retries=0},json);marker.Flush();
 try{
  if(call.Arm=="map"){
   var catalog=Load<FragmentCatalog>($"fragments/{call.Pair}.json");CheckCatalog(input.Input.Content,catalog);
   var ai=client.GetChatClient("gpt-6-luna").AsIChatClient().AsAIAgent(File.ReadAllText(Path.Combine(output,"prompts/mapper.md")),"Mapper");
   var payload=JsonSerializer.Serialize(new{originalSource=input.Input.Content,selectedIdea=input.SelectedIdea.Idea,sourceFragments=catalog.Fragments},json);
   var response=await ai.RunAsync(payload,null,new ChatClientAgentRunOptions(new ChatOptions{MaxOutputTokens=6000,ResponseFormat=ChatResponseFormat.ForJsonSchema<EvidenceMap>(json,nameof(EvidenceMap))}));
   File.WriteAllText(Path.Combine(dir,"map-raw.json"),response.Text);
   var map=JsonSerializer.Deserialize<EvidenceMap>(response.Text,json)??throw new Exception("Null map");var errors=ValidateMap(map,catalog);Save(dir,"anchor-validation.json",errors);
   if(errors.Count>0)throw new Exception("Invalid map anchors/contract; no repair");
   Save(dir,"map.json",new{items=map.Items.Select(i=>new{i.Id,i.Kind,i.Proposition,i.SourceFragmentIds,sourceQuotes=i.SourceFragmentIds.Select(fid=>catalog.Fragments.Single(f=>f.Id==fid).Text).ToArray(),i.Materiality,i.Boundary}),map.IdeaChecks,map.Limitations});
   Console.WriteLine(call.Id+": valid controlled anchors, items="+map.Items.Count);
  }else{
   var research=Load<ResearchResult>($"cases/{call.Pair}/{call.Variant}/research-result.json");
   File.WriteAllText(Path.Combine(dir,"synthesis-filter-output.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content));
   var mapText=call.Arm=="with"?File.ReadAllText(Path.Combine(output,"calls",call.Pair+"-map","map.json")):null;
   var ai=new MapClient(client.GetChatClient("gpt-6-luna").AsIChatClient(),mapText).AsAIAgent(File.ReadAllText(Path.Combine(output,"prompts/judge.md")),"Judge");
   var verdict=await new ResearchSemanticJudge(ai).EvaluateAsync(input,research);Save(dir,"verdict.json",verdict);
   Console.WriteLine(call.Id+": accepted="+verdict.Accepted+", issues="+verdict.BlockingIssues.Count);
  }
 }catch(Exception ex){File.WriteAllText(Path.Combine(dir,"error.txt"),ex.ToString());Console.WriteLine(call.Id+": FAILED "+ex.Message);}
}
T Load<T>(string p)=>JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(output,p)),json)!;
void Save(string dir,string name,object value)=>File.WriteAllText(Path.Combine(dir,name),JsonSerializer.Serialize(value,json));
static string Hash(string source)=>Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(source))).ToLowerInvariant();
static void CheckCatalog(string source,FragmentCatalog catalog){
 if(catalog.OffsetUnit!="UTF-16 code units"||catalog.SourceSha256Utf8!=Hash(source))throw new Exception("Source hash/unit mismatch");
 int end=0;var ids=new HashSet<string>();foreach(var f in catalog.Fragments){if(!ids.Add(f.Id)||f.Start!=end||f.Length<=0||f.Start+f.Length>source.Length||!string.Equals(source.Substring(f.Start,f.Length),f.Text,StringComparison.Ordinal))throw new Exception("Invalid controlled span");end+=f.Length;}
 if(end!=source.Length||!string.Equals(string.Concat(catalog.Fragments.Select(f=>f.Text)),source,StringComparison.Ordinal))throw new Exception("Roundtrip mismatch");
}
static List<string> ValidateMap(EvidenceMap map,FragmentCatalog catalog){
 var errors=new List<string>();var ids=new HashSet<string>();var fragments=catalog.Fragments.Select(f=>f.Id).ToHashSet(StringComparer.Ordinal);
 foreach(var i in map.Items){
  if(string.IsNullOrWhiteSpace(i.Id)||!ids.Add(i.Id))errors.Add("Duplicate/empty item ID");
  if(!new[]{"claim","explicitMeaning","causalLimit","counterexample","negativeControl"}.Contains(i.Kind))errors.Add(i.Id+": invalid kind");
  if(new[]{i.Proposition,i.Materiality,i.Boundary}.Any(string.IsNullOrWhiteSpace))errors.Add(i.Id+": incomplete fields");
  if(i.SourceFragmentIds.Count==0||i.SourceFragmentIds.Distinct(StringComparer.Ordinal).Count()!=i.SourceFragmentIds.Count||i.SourceFragmentIds.Any(id=>!fragments.Contains(id)))errors.Add(i.Id+": invalid fragment IDs");
 }
 foreach(var c in map.IdeaChecks)if(!new[]{"supported","qualified","unsupported"}.Contains(c.Status)||c.SourceItemIds.Any(id=>!ids.Contains(id)))errors.Add("Invalid idea check");
 if(map.Items.Count==0)errors.Add("Empty map");return errors;
}
sealed record CallPlan(string Id,string Pair,string Variant,string Arm);
sealed record Fragment(string Id,int Start,int Length,string Text);
sealed record FragmentCatalog(string SourceSha256Utf8,string OffsetUnit,List<Fragment> Fragments);
sealed class MapClient(IChatClient inner,string? map):DelegatingChatClient(inner){
 public override Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default){
  var updated=messages.Select(m=>{if(m.Role!=ChatRole.User)return m;var node=JsonNode.Parse(m.Text!)!.AsObject();node["originalSource"]=node["source"]!.DeepClone();node.Remove("source");if(map!=null)node["evidenceMap"]=JsonNode.Parse(map);return new ChatMessage(ChatRole.User,node.ToJsonString());}).ToArray();
  options??=new ChatOptions();options.MaxOutputTokens=6000;return base.GetResponseAsync(updated,options,cancellationToken);
 }
}
`;
fs.writeFileSync(path.join(out,'runner/Program.cs'),imports+main+classes);
