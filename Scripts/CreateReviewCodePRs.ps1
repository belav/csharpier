# This is used to create PRs against a number of forked public repos so that csharpier can be run against them
# and the formatting reviewed before releasing csharpier
# Long term this process can probably go away, but for now it is a tolerable way to find edge cases or bugs that are introduced

param (
    [Parameter(Mandatory=$true)]
    [string]$version
)

# aspnetcore
# AspNetWebStack
# AutoMapper
# command-line-api
# Core
# efcore
# format
# insite-commerce
# moq4
# Newtonsoft.Json
# roslyn
# runtime

. $PsScriptRoot/Helpers.ps1

$ErrorActionPreference = "Stop"

if ($version -eq "") {
    Write-Output "need version"
    exit 1
}

$csharpierProject = "C:\Projects\csharpier"
$csharpierRepos = "C:\Projects\csharpierForkedRepos"

Build-CSharpier

$versionWithQuotes = "`"" + $version + "`"";

$prs = @()

foreach($folder in Get-ChildItem $csharpierRepos) {
    Push-Location $folder.FullName
    Write-Output $folder.FullName

    & git checkout main
    & git reset --hard
    & git branch -d $version
    & git checkout -b $version
}

Push-Location $csharpierRepos

dotnet $csharpierDllPath .

foreach($folder in Get-ChildItem $csharpierRepos)
{
    Push-Location $folder.FullName
    Write-Output $folder.FullName
    
    & git add -A
    & git commit -m $versionWithQuotes
    & git push --set-upstream origin $version
    #uses https://github.com/github/hub/releases
    $newPr = & hub pull-request -b belav:main -m $versionWithQuotes
    $prs += $newPr
    Pop-Location
}


Write-Output $prs