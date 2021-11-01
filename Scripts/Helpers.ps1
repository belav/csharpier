$repositoryRoot = Join-Path $PSScriptRoot ".."
$csProjectPath = Join-Path $repositoryRoot "Src/CSharpier.CLI/CSharpier.CLI.csproj"
$csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier.CLI/bin/release/net5.0/dotnet-csharpier.dll"

function Build-CSharpier
{
    & dotnet build $csProjectPath -c release
    if ($lastExitCode -gt 0) {
        exit $lastExitCode;
    }
}