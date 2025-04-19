function CSH-BuildPackages() {
    $repositoryRoot = Join-Path $PSScriptRoot ".."

    # build individual projects because CSharpier.MSBuild can't be in the sln
    $projects = @("CSharpier.Cli", "CSharpier.Core", "CSharpier.MSBuild")
    $nupkgPath = Join-Path $repositoryRoot "nupkg"

    foreach ($project in $projects) {
        $csProjectPath = Join-Path $repositoryRoot "Src/$($project)/$($project).csproj"
        & dotnet pack $csProjectPath -c release --output $nupkgPath
    }    
}

Export-ModuleMember -Function CSH-*
