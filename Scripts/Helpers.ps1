$repositoryRoot = Join-Path $PSScriptRoot ".."
$csProjectPath = Join-Path $repositoryRoot "Src/CSharpier/CSharpier.csproj"
$csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier/bin/release/net6.0/dotnet-csharpier.dll"

function Build-CSharpier
{
    & dotnet build $csProjectPath -c release
    if ($lastExitCode -gt 0) {
        exit $lastExitCode;
    }
}