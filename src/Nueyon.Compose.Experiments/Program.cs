using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Nueyon.Compose.Experiments;
using Nueyon.Compose.Host.Console;
using Nueyon.Compose.Infrastructure.Options;
using OpenAI;

try
{
    if (args.Length < 2 || (args[0] == "offline" && args.Length != 2) ||
        (args[0] == "run" && args.Length != 5) || args[0] is not ("offline" or "run"))
        throw new ArgumentException("Usage: offline <experimentRoot> | run <experimentRoot> <caseId> <A|B|C> <replicate>");
    var root = Path.GetFullPath(args[1]);
    var verification = Path.Combine(root, "verification");
    Directory.CreateDirectory(verification);
    if (args[0] == "offline")
    {
        FreezeVerifier.VerifyBaseline(root);
        if (File.Exists(Path.Combine(verification, "frozen.json"))) FreezeVerifier.Verify(root);
        await OfflineChecks.RunAsync();
        File.WriteAllText(Path.Combine(verification, "evidence.schema.json"), ExperimentValidator.Schema(false).GetRawText());
        File.WriteAllText(Path.Combine(verification, "review.schema.json"), ExperimentValidator.Schema(true).GetRawText());
        Console.WriteLine("Offline checks passed.");
        return 0;
    }

    using var runLock = new FileStream(Path.Combine(verification, "run.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    FreezeVerifier.Verify(root);
    FreezeVerifier.VerifyBaseline(root);
    var config = new ConfigurationBuilder()
        .SetBasePath(Path.GetFullPath(Path.Combine("src", "Nueyon.Compose.Host.Console")))
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .AddEnvironmentVariables().AddUserSecrets(typeof(Cli).Assembly, true).Build();
    var settings = config.GetSection("OpenAI").Get<OpenAiOptions>() ?? new OpenAiOptions();
    settings.Model = "gpt-5-mini";
    settings.Validate();
    var openAi = new OpenAIClient(new ApiKeyCredential(settings.ApiKey), new OpenAIClientOptions
    {
        RetryPolicy = new ClientRetryPolicy(0), NetworkTimeout = TimeSpan.FromSeconds(150)
    });
    using var client = openAi.GetChatClient(settings.Model).AsIChatClient();
    var ledger = new CallLedger(Path.Combine(verification, "call-ledger.jsonl"));
    var cell = new CellState(args[2], args[3], int.Parse(args[4], System.Globalization.CultureInfo.InvariantCulture));
    var result = await new ExperimentRunner(root, client, ledger).RunAsync(cell, false);
    Console.WriteLine(result);
    return result is "success" or "skipped" ? 0 : 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);
    return 1;
}
