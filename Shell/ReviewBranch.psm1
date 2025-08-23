function CSH-ReviewBranch {
    param (
        [string]$folder,
        [string]$pathToTestingRepo,
        [switch]$skipValidation = $true,
        [switch]$onlyXml = $false
    )

    $repositoryRoot = Join-Path $PSScriptRoot ".."
    $csProjectPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/CSharpier.Cli.csproj"
    $csharpierDllPath = Join-Path $repositoryRoot "Src/CSharpier.Cli/bin/release/net8.0/CSharpier.dll"

    $location = Get-Location

    Set-Location $repositoryRoot
    
    if (!$pathToTestingRepo) {
        $pathToTestingRepo = Join-Path $PSScriptRoot "../../csharpier-repos"
    }
    if ($folder -ne $null) {
        $pathToTestingRepo = Join-Path $pathToTestingRepo $folder 
    }
    
    if (!(Test-Path $pathToTestingRepo)) {
        Write-Output "No directory found at $($pathToTestingRepo)."
        Write-Output "Please checkout out https://github.com/belav/csharpier-repos there or supply -pathToTestingRepo"
        return
    }

    $ErrorActionPreference = "Stop"

    $branch = & git branch --show-current

    if ($branch -eq "main") {
        Write-Output "You must be on the branch you want to test. You are currently on main"
        return
    }

    $preBranch = "pre-" + $branch
    $postBranch = "post-" + $branch

    if ($folder -ne $null) {
        $preBranch += "-" + $folder
        $postBranch += "-" + $folder
    }

    Set-Location $pathToTestingRepo
    & git reset --hard *> $null
    & git pull
    try {
        & git checkout $postBranch 2>&1
    }
    catch { }
    $postBranchOutput = (git status 2>&1) | Out-String
    $firstRun = -not $postBranchOutput.Contains("On branch $postBranch")

    $skipValidationParam = ""
    if ($skipValidation -eq $true) {
        $skipValidationParam = "--skip-validation"
    }

    if ($firstRun) {
        Set-Location $repositoryRoot

        & git checkout main #2>&1 | Out-String

        CSH-BuildProject -doNotExit

        if ($lastExitCode -gt 0) {
            return
        }

        Set-Location $pathToTestingRepo

        & git checkout main
        & git reset --hard
        & git checkout -b $preBranch

        if ($onlyXml) {
            Add-Content -Path "$pathToTestingRepo/.csharpierignore" -Value "**/*.cs"
        }
        
        dotnet $csharpierDllPath format . $skipValidationParam --no-cache
        # there is some weirdness with a couple files with #if where
        # they need to be formatted twice to get them stable
        # it isn't worth fixing in csharpier, because it only really affects this
        dotnet $csharpierDllPath format . $skipValidationParam

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

    dotnet $csharpierDllPath format . $skipValidationParam --no-cache

    & git add -A
    & git commit -m "After $branch"
    & git push --set-upstream origin $postBranch

    if ($firstRun) {
        # uses https://github.com/github/hub/releases
        # if this hangs run a command against this repo outside of here, it will store
        # a token. Or I can just have a way to pass a GITHUB_TOKEN or store it in a .env
        $newPr = & hub pull-request -b belav:$preBranch -m "Testing $branch $folder"
        Write-Output $newPr
    }

    Set-Location $location
}

Export-ModuleMember -Function CSH-*