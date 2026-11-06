param([Parameter(Mandatory=$true)][string]$Root)
$ErrorActionPreference = 'Stop'
$assemblyPath = Join-Path $Root 'src\Nueyon.Compose.Application\bin\Debug\net10.0\Nueyon.Compose.Application.dll'
$context = [System.Runtime.Loader.AssemblyLoadContext]::new('prompt-check-' + [guid]::NewGuid().ToString('N'), $true)
try {
    $assembly = $context.LoadFromAssemblyPath($assemblyPath)
    $stream = $assembly.GetManifestResourceStream('Nueyon.Compose.Application.Prompts.Narrative.Narrative.system.prompt.txt')
    if ($null -eq $stream) { throw 'Embedded Narrative system prompt resource not found.' }
    $memory = [IO.MemoryStream]::new()
    try { $stream.CopyTo($memory); [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($memory.ToArray())) }
    finally { $stream.Dispose(); $memory.Dispose() }
}
finally { $context.Unload() }
