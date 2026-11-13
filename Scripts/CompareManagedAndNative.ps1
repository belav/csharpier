# Formats the same set of files with the managed and the NativeAOT builds of csharpier and fails if the results differ.
# Run from the root, IE
# pwsh ./Scripts/CompareManagedAndNative.ps1 -ManagedDll ./Src/CSharpier.Cli/bin/Release/net10.0/CSharpier.dll -NativeExecutable ./publish/CSharpier.exe
param(
    [Parameter(Mandatory = $true)][string]$ManagedDll,
    [Parameter(Mandatory = $true)][string]$NativeExecutable
)

$ErrorActionPreference = "Stop"

$repositoryRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$ManagedDll = (Resolve-Path $ManagedDll).Path
$NativeExecutable = (Resolve-Path $NativeExecutable).Path

$workingDirectory = Join-Path ([System.IO.Path]::GetTempPath()) ("CSharpierCompare" + [Guid]::NewGuid().ToString("N"))
$managedDirectory = Join-Path $workingDirectory "managed"
$nativeDirectory = Join-Path $workingDirectory "native"
New-Item $managedDirectory -ItemType Directory | Out-Null

$extensionsByFolder = @{ "cs" = ".cs"; "csx" = ".csx"; "xml_ignore" = ".xml"; "xml_strict" = ".xml" }
$testFilesRoot = Join-Path $repositoryRoot "Src/CSharpier.Tests/FormattingTests/TestFiles"
foreach ($folder in $extensionsByFolder.Keys) {
    $destination = Join-Path $managedDirectory "TestFiles/$folder"
    New-Item $destination -ItemType Directory | Out-Null
    Get-ChildItem (Join-Path $testFilesRoot $folder) -Filter "*.test" |
        Where-Object { $_.Name -notlike "*.actual.test" -and $_.Name -notlike "*.expected.test" } |
        ForEach-Object {
            Copy-Item $_.FullName (Join-Path $destination ($_.BaseName + $extensionsByFolder[$folder]))
        }
}

$sourceRoot = Join-Path $repositoryRoot "Src"
Get-ChildItem $sourceRoot -Recurse -File -Include "*.cs", "*.csproj", "*.props", "*.targets" |
    Where-Object { $_.FullName -notmatch "[\\/](bin|obj|node_modules|TestFiles)[\\/]" } |
    ForEach-Object {
        $destination = Join-Path $managedDirectory ("Src/" + [System.IO.Path]::GetRelativePath($sourceRoot, $_.FullName))
        New-Item (Split-Path $destination) -ItemType Directory -Force | Out-Null
        Copy-Item $_.FullName $destination
    }

Set-Content (Join-Path $managedDirectory ".editorconfig") "root = true"
Copy-Item $managedDirectory $nativeDirectory -Recurse

$arguments = @("format", ".", "--no-cache", "--no-msbuild-check", "--include-generated")

Push-Location $managedDirectory
$managedOutput = & dotnet $ManagedDll @arguments 2>&1 | Out-String
$managedExitCode = $LASTEXITCODE
Pop-Location

Push-Location $nativeDirectory
$nativeOutput = & $NativeExecutable @arguments 2>&1 | Out-String
$nativeExitCode = $LASTEXITCODE
Pop-Location

$failures = @()
if ($managedExitCode -ne $nativeExitCode) {
    $failures += "Exit codes differ: managed $managedExitCode, native $nativeExitCode"
}

# the last line of the output includes the elapsed time
$normalizedManagedOutput = ($managedOutput -replace "in \d+ms", "in Xms").Replace($managedDirectory, "")
$normalizedNativeOutput = ($nativeOutput -replace "in \d+ms", "in Xms").Replace($nativeDirectory, "")
if ($normalizedManagedOutput -ne $normalizedNativeOutput) {
    $failures += "Console output differs.`n--- managed`n$managedOutput`n--- native`n$nativeOutput"
}

$managedFiles = Get-ChildItem $managedDirectory -Recurse -File
foreach ($managedFile in $managedFiles) {
    $relativePath = [System.IO.Path]::GetRelativePath($managedDirectory, $managedFile.FullName)
    $nativeFile = Join-Path $nativeDirectory $relativePath
    if ((Get-FileHash $managedFile.FullName).Hash -ne (Get-FileHash $nativeFile).Hash) {
        $failures += "File differs: $relativePath"
    }
}

Write-Host "Compared $($managedFiles.Count) files. Managed: $($managedOutput.Trim().Split("`n")[-1].Trim())"

if ($failures.Count -gt 0) {
    $failures | ForEach-Object { Write-Host $_ }
    Write-Host "The formatted files were left in $workingDirectory"
    exit 1
}

Remove-Item $workingDirectory -Recurse -Force
Write-Host "The managed and native builds produced the same results."
