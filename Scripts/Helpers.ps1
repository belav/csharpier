$repositoryRoot = Join-Path $PSScriptRoot ".."
$csProjectPath = Join-Path $repositoryRoot "Src/CSharpier/CSharpier.csproj"
$csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier/bin/release/net5.0/dotnet-csharpier.dll"

function Build-CSharpier
{
    & dotnet build $csProjectPath -c release
}