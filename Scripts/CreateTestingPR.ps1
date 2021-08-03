$ErrorActionPreference = "Stop"

$branch = & git branch --show-current

if ($branch -eq "master") {
    Write-Output "You must be on the branch you want to test. You are currently on master"
    exit 1
}

$preBranch = "pre-" + $branch
$postBranch = "post-" + $branch

$csharpierProject = (Get-Item $PSScriptRoot).Parent.FullName

# TODO this should probably be a parameter
$testingRepo = "C:\Projects\csharpierForkedRepos\aspnetcore"

Set-Location $testingRepo
& git reset --hard

git checkout $postBranch
$postBranchOutput = (git status) | Out-String
$firstRun = -not $postBranchOutput.Contains("On branch $postBranch")
if ($firstRun)
{
    Set-Location $csharpierProject
    # TODO this should make sure the working tree is clean
    & git checkout master
    & dotnet build Src\CSharpier\CSharpier.csproj -c Release

    Set-Location $testingRepo
    
    & git checkout main
    & git reset --hard
    & git checkout -b $preBranch

    dotnet $csharpierProject\Src\CSharpier\bin\Release\net5.0\dotnet-csharpier.dll .

    & git add -A
    & git commit -m "Before $branch"
    & git push --set-upstream origin $preBranch
}

Set-Location $csharpierProject
git checkout $branch
& dotnet build Src\CSharpier\CSharpier.csproj -c Release

Set-Location $testingRepo

if ($firstRun) {
    & git checkout -b $postBranch
} else {
    & git checkout $postBranch
}

dotnet $csharpierProject\Src\CSharpier\bin\Release\net5.0\dotnet-csharpier.dll .

& git add -A
& git commit -m "After $branch"
& git push --set-upstream origin $postBranch

if ($firstRun) {
    #uses https://github.com/github/hub/releases
    $newPr = & hub pull-request -b belav:$preBranch -m "Testing $branch"
    Write-Output $newPr
}
