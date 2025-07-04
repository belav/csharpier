param
(
    [Parameter(Mandatory = $true)] [String]$BranchName
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

$skippedDirectories = @("node_modules",   `
          "dist",   `
          "obj",   `
          "bin",   `
          "lib",   `
          "Libraries",   `
          "packages",   `
          "Service References",   `
          ".idea",   `
          ".vscode",   `
          "BuildArtifacts",   `
          ".sass-cache",   `
          "Typings",   `
          ".git",   `
          ".sql"   `
    ) `
    | ForEach-Object { ([IO.Path]::DirectorySeparatorChar + $_).ToLower() }

Write-Host "::group::Checking for TODOs"

$directoriesToProcess = @()
foreach ($item in Get-ChildItem -Recurse -Directory) {
    $add = $true
    foreach ($directory in $skippedDirectories) {
        if ($item.FullName.ToLower() -like "*$directory*") {
            $add = $false
            continue
        }
    }

    if ($add) {
        $directoriesToProcess += $item.FullName
    }
}

Write-Host "Looking for TODO $BranchName"
$regexPattern = "(TODO[\s-\:]*$BranchName)"

function checkForTodoComments {
    $directoriesToProcess | ForEach-Object -Parallel {
        foreach ($item in Get-ChildItem -Path "$_\*" -File -Exclude "*.exe", "*.dll", "*.pdf") {
            if ($item.FullName.EndsWith(".js") -or $item.FullName.EndsWith(".js.map")) {
                if (Test-Path $item.FullName.Replace(".js.map", ".ts").Replace(".js", ".ts")) {
                    continue
                }
            }

            Select-String -Pattern "$using:regexPattern" -Path $item.FullName
        }
    }
}

$todoMatches = checkForTodoComments

Write-Host "::endgroup::"
if ($null -eq $todoMatches) {
    exit 0
}

Write-Host "`nFound TODOs that need to be addressed."
foreach ($todoMatch in $todoMatches) {
    $relativePath = $todoMatch.Path.Substring($PWD.ToString().Length + 1)
    Write-Host "::error file=$relativePath,line=$( $todoMatch.LineNumber )::This TODO needs to be cleaned up"
    Write-Host "       $( $relativePath ):$( $todoMatch.LineNumber )"
    Write-Host "       $( $todoMatch.Line )"
}

exit 1
