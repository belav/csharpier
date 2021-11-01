$repositoryRoot = Join-Path $PSScriptRoot ".."

$projects = @("CSharpier.CLI", "CSharpier", "CSharpier.MSBuild")
$nupkgPath = Join-Path $repositoryRoot "nupkg"

foreach ($project in $projects) {
    $csProjectPath = Join-Path $repositoryRoot "Src/$($project)/$($project).csproj"
    & dotnet pack $csProjectPath -c release --output $nupkgPath
}

