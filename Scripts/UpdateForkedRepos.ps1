param (
    [string]$version
) 

if ($version -eq "") {
    Write-Output "need version"
    exit 1
}
$csharpierRepos = "C:\Projects\csharpierForkedRepos"

$versionWithQuotes = "`"" + $version + "`"";

foreach($folder in Get-ChildItem $csharpierRepos) {
    Write-Output $folder.FullName

    Push-Location $folder.FullName
    # TODO this should probably use a local verison of the tool, so the input version for sure matches
    # but if we do that, we do need a prerelease version for the CreateReviewCodePRs.ps1
    # TODO a lot of the git commands look like they fail, but really succeed, wtf?
    & git checkout main
    & git reset --hard

    dotnet csharpier

    & git add -A
    & git commit -m $versionWithQuotes
    & git tag $version
    & git push origin
    & git push origin --tags
    Pop-Location
}