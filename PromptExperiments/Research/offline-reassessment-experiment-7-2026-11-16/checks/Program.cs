using System.Text.Json;
using Nueyon.Compose.Application.Agents.Synthesis;
var experiment = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,"../../../../../production-validation-experiment-6-2026-11-12"));
var checks = new List<object>();
foreach(var dir in Directory.GetDirectories(Path.Combine(experiment,"calls"),"*-Research")) {
 var content=File.ReadAllText(Path.Combine(dir,"content.md"));
 var expected=File.ReadAllText(Path.Combine(dir,"projection.md"));
 var actual=SynthesisResearchFilter.ExtractFactsAndUnknowns(content);
 bool match=actual.Replace("\r\n","\n").TrimEnd()==expected.Replace("\r\n","\n").TrimEnd();
 if(!match)throw new Exception(Path.GetFileName(dir));
 checks.Add(new {run=Path.GetFileName(dir),productionFilterMatch=match});
}
if(checks.Count!=12)throw new Exception("Expected exactly twelve");
Console.WriteLine(JsonSerializer.Serialize(checks,new JsonSerializerOptions{WriteIndented=true}));
