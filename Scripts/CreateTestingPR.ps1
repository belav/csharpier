param (
    [string]$pathToTestingRepo,
    [switch] $fast = $false
)

if (!$pathToTestingRepo) {
    $pathToTestingRepo = "C:\Projects\csharpier-repos"
}
if (!(Test-Path $pathToTestingRepo)) {
    Write-Output "No directory found at $($pathToTestingRepo)."
    Write-Output "Please checkout out https://github.com/belav/csharpier-repos there or supply -pathToTestingRepo"
    exit 1
}

. $PsScriptRoot/Helpers.ps1

$ErrorActionPreference = "Stop"

$branch = & git branch --show-current

if ($branch -eq "master") {
    Write-Output "You must be on the branch you want to test. You are currently on master"
    exit 1
}

$preBranch = "pre-" + $branch
$postBranch = "post-" + $branch

Set-Location $pathToTestingRepo
& git reset --hard

git checkout $postBranch
$postBranchOutput = (git status) | Out-String
$firstRun = -not $postBranchOutput.Contains("On branch $postBranch")

$fastParam = ""
if ($fast -eq $true) {
    $fastParam = "--fast"
}

if ($firstRun)
{
    Set-Location $repositoryRoot
    # TODO this should make sure the working tree is clean
    & git checkout master
    Build-CSharpier

    Set-Location $pathToTestingRepo
    
    & git checkout main
    & git reset --hard
    & git checkout -b $preBranch
    
    dotnet $csharpierDllPath . $fastParam

    & git add -A
    & git commit -m "Before $branch"
    & git push --set-upstream origin $preBranch
}

Set-Location $repositoryRoot
git checkout $branch
Build-CSharpier

Set-Location $pathToTestingRepo

if ($firstRun) {
    & git checkout -b $postBranch
} else {
    & git checkout $postBranch
}

dotnet $csharpierDllPath . $fastParam

& git add -A
& git commit -m "After $branch"
& git push --set-upstream origin $postBranch

if ($firstRun) {
    #uses https://github.com/github/hub/releases
    $newPr = & hub pull-request -b belav:$preBranch -m "Testing $branch"
    Write-Output $newPr
}

Pop-Location