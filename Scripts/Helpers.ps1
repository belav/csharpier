$repositoryRoot = Join-Path $PSScriptRoot ".."
$csProjectPath = Join-Path $repositoryRoot "Src/CSharpier/CSharpier.csproj"
# TODO 10 should we test multiple versions? We should probably not only target .net6
$csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier/bin/release/net6.0/dotnet-csharpier.dll"

function Build-CSharpier
{
    & dotnet build $csProjectPath -c release
    if ($lastExitCode -gt 0) {
        exit $lastExitCode;
    }
}