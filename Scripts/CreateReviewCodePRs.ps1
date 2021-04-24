# This is used to create PRs against a number of forked public repos so that csharpier can be run against them
# and the formatting reviewed before releasing csharpier
# Long term this process can probably go away, but for now it is a good way to find edge cases or bugs that are introduced

param (
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

if ($version -eq "") {
    Write-Output "need version"
    exit 1
}
$csharpierRepo = "C:\Projects\csharpier"
$csharpierRepos = "C:\Projects\csharpierForkedRepos"

# TODO also add another script for basically updating these with the actual release
# should that script merge PRs? or just format directly on master? Maybe closing the existing PRs?
& dotnet build $csharpierRepo\Src\CSharpier\CSharpier.csproj -c Release

$versionWithQuotes = "`"" + $version + "`"";

foreach($folder in Get-ChildItem $csharpierRepos) {
    Write-Output $folder.FullName

    Set-Location $folder.FullName

    #TODO a lot of the git commands look like they fail, but really succeed, wtf?
    & git checkout main
    & git reset --hard
    & git branch -d $version
    & git checkout -b $version

    dotnet $csharpierRepo\Src\CSharpier\bin\Release\net5.0\dotnet-csharpier.dll

    & git add -A
    & git commit -m $versionWithQuotes
    & git push --set-upstream origin $version
    #uses https://github.com/github/hub/releases
    & hub pull-request -b belav:main -m $versionWithQuotes
    # TODO keep track of the PRs created to write out all the links at the end
}