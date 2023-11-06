function CSH-CreateRelease {
    param (
        [Parameter(Mandatory=$true)]
        [string]$versionNumber
    )

    $previousVersionNumber

    git checkout main
    git pull
    git checkout -b "release-$versionNumber"
    
    $versionPropsPath = $PSScriptRoot + "/../Nuget/Build.props"
    $versionProps = [xml](Get-Content $versionPropsPath)

    $previousVersionNumber = $versionProps.Project.PropertyGroup.Version
    $versionProps.Project.PropertyGroup.Version = $versionNumber
    $versionProps.Save($versionPropsPath)

    $changeLog = CSH-ChangeLog $previousVersionNumber $versionNumber

    $changeLogPath = ($PSScriptRoot + "/../CHANGELOG.md")

    $changeLogValue = [IO.File]::ReadAllText($changeLogPath)

    Set-Content -Encoding UTF8 -Path $changeLogPath -Value ($changeLog + $changeLogValue)

    foreach ($file in Get-ChildItem ($PSScriptRoot + "/../Docs") -Filter "*.md")
    {
        Copy-Item $file.FullName ($PSScriptRoot + "/../Src/Website/docs/" + $file.Name)
    }
}

Export-ModuleMember -Function CSH-*