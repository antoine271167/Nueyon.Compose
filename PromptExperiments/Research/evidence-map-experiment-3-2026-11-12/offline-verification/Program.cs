using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using Nueyon.Compose.Application.Agents.Research;
using Nueyon.Compose.Application.Agents.Synthesis;
using Nueyon.Compose.Domain;
var output=Path.GetFullPath(args.Single());
var json=new JsonSerializerOptions(JsonSerializerDefaults.Web){PropertyNameCaseInsensitive=true,WriteIndented=true};
var results=new List<object>();
foreach(var dir in Directory.GetDirectories(Path.Combine(output,"calls"))){
 var id=Path.GetFileName(dir);if(id.EndsWith("-map"))continue;
 var variant=id.EndsWith("-correct")?"correct":"bad";
 var pair=id[..^(variant.Length+1)];
 var input=JsonSerializer.Deserialize<ResearchInput>(File.ReadAllText(Path.Combine(output,"cases",pair,variant,"input.json")),json)!;
 var research=JsonSerializer.Deserialize<ResearchResult>(File.ReadAllText(Path.Combine(output,"cases",pair,variant,"research-result.json")),json)!;
 using var raw=JsonDocument.Parse(File.ReadAllText(Path.Combine(dir,"response-raw.json")));
 var text=raw.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()!;
 bool parsed=true;string? error=null;ResearchVerdict? verdict=null;
 try{verdict=await new ResearchSemanticJudge(new ReplayClient(text).AsAIAgent("Offline replay; no network client")).EvaluateAsync(input,research);}catch(Exception ex){parsed=false;error=ex.Message;}
 bool liveParsed=File.Exists(Path.Combine(dir,"verdict.json"));
 bool same=parsed==liveParsed&&(!parsed||JsonSerializer.Serialize(verdict,json)==File.ReadAllText(Path.Combine(dir,"verdict.json")));
 bool filter=SynthesisResearchFilter.ExtractFactsAndUnknowns(research.Content)==File.ReadAllText(Path.Combine(dir,"synthesis-filter-output.md"));
 results.Add(new{id,parsed,error,sameAsLive=same,actualProductionFilterMatches=filter});
 if(!same||!filter)throw new Exception("Replay mismatch: "+id);
}
File.WriteAllText(Path.Combine(output,"verification-offline-replay.json"),JsonSerializer.Serialize(results,json));
Console.WriteLine("Offline replay/filter checks passed: "+results.Count+"; no network client instantiated.");
sealed class ReplayClient(string text):IChatClient{
 public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default)=>Task.FromResult(new ChatResponse([new ChatMessage(ChatRole.Assistant,text)]));
 public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages,ChatOptions? options=null,CancellationToken cancellationToken=default)=>throw new NotSupportedException();
 public object? GetService(Type type,object? key=null)=>null;
 public void Dispose(){}
}
