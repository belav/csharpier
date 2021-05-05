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
    if ($folder.FullName.Contains("aspnetcore")) {
        continue
    }
    Write-Output $folder.FullName

    Push-Location $folder.FullName

    #TODO a lot of the git commands look like they fail, but really succeed, wtf?
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