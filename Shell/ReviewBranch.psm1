function CSH-ReviewBranch {
    param (
        [string]$pathToTestingRepo,
        [switch]$fast = $true
    )

    $repositoryRoot = Join-Path $PSScriptRoot ".."
    $csProjectPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/CSharpier.Cli.csproj"
    $csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/bin/release/net7.0/dotnet-csharpier.dll"
    
    if (!$pathToTestingRepo) {
        $pathToTestingRepo = "C:\Projects\csharpier-repos"
    }
    if (!(Test-Path $pathToTestingRepo)) {
        Write-Output "No directory found at $($pathToTestingRepo)."
        Write-Output "Please checkout out https://github.com/belav/csharpier-repos there or supply -pathToTestingRepo"
        exit 1
    }

    $ErrorActionPreference = "Stop"

    $branch = & git branch --show-current

    if ($branch -eq "main") {
        Write-Output "You must be on the branch you want to test. You are currently on main"
        exit 1
    }

    $preBranch = "pre-" + $branch
    $postBranch = "post-" + $branch

    $location = Get-Location
    
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
        & git checkout main
        CSH-BuildProject

        Set-Location $pathToTestingRepo

        & git checkout main
        & git reset --hard
        & git checkout -b $preBranch

        dotnet $csharpierDllPath . $fastParam --no-cache
        # there is some weirdness with a couple files with #if where
        # they need to be formatted twice to get them stable
        # it isn't worth fixing in csharpier, because it only really affects this
        dotnet $csharpierDllPath . $fastParam

        & git add -A
        & git commit -m "Before $branch"
        & git push --set-upstream origin $preBranch
    }

    Set-Location $repositoryRoot
    git checkout $branch
    CSH-BuildProject

    Set-Location $pathToTestingRepo

    if ($firstRun) {
        & git checkout -b $postBranch
    } else {
        & git checkout $postBranch
    }

    dotnet $csharpierDllPath . $fastParam --no-cache

    & git add -A
    & git commit -m "After $branch"
    & git push --set-upstream origin $postBranch

    if ($firstRun) {
        #uses https://github.com/github/hub/releases
        $newPr = & hub pull-request -b belav:$preBranch -m "Testing $branch"
        Write-Output $newPr
    }

    Set-Location $location
}

Export-ModuleMember -Function CSH-*