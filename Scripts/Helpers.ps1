$repositoryRoot = Join-Path $PSScriptRoot ".."
$csProjectPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/CSharpier.Cli.csproj"
$csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/bin/release/net6.0/dotnet-csharpier.dll"

function Build-CSharpier
{
    & dotnet build $csProjectPath -c release
    if ($lastExitCode -gt 0) {
        exit $lastExitCode;
    }
}