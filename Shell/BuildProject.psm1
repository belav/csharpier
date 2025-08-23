function CSH-BuildProject {
    param (
        [switch]$doNotExit = $false
    )
    
    
    $repositoryRoot = Join-Path $PSScriptRoot ".."
    $csProjectPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/CSharpier.Cli.csproj"
    
    & dotnet build $csProjectPath -c release
    if ($lastExitCode -gt 0 -and -not $doNotExit) {
        exit $lastExitCode;
    }
}

Export-ModuleMember -Function CSH-*