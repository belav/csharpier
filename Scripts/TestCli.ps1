$ErrorActionPreference = "Stop"

. $PsScriptRoot/Helpers.ps1

if (-not (Test-Path $csharpierDllPath)) {
    Write-Output "No dll found at $($csharpierDllPath)" 
    exit 1
}

if (-not (Test-Path "TestCli"))
{
    New-Item "TestCli" -ItemType "Directory"
}

Set-Location "TestCli"

$formatted = "public class ClassName { }"
$unformatted = "public class ClassName  { }"

$failed = $false;

Write-Output "---- File provided by stdIn should be formatted"
$stdInResult = $unformatted | dotnet $csharpierDllPath
if ($stdInResult -ne $formatted) {
    Write-Output "Piping c# code to CSharpier did not result in formatted code. The result was:"
    Write-Output $stdInResult
    Write-Output ""
    $failed = $true;
}

# TODO prettier does not just spit out the file, the extension does not use the CLI
# it can print out detailed info about what went wrong
# the prettier CLI also shows errors when you pipe to it
# have different modes? like --mode plugin, then later can add --mode plugin2

Write-Output "---- File with compilation error provided by stdIn should not be changed"
$stdInResult = "public class ClassName { " | dotnet $csharpierDllPath
if ($stdInResult -ne "public class ClassName { ") {
    Write-Output "Piping invalid c# code to CSharpier did not result in the original code. The result was:"
    Write-Output $stdInResult
    Write-Output ""
    $failed = $true;
}

Write-Output "---- Check should exit with 1 when file not formatted"
New-Item -Path . -Name "Check.cs" -Value $unformatted 2>&1 | Out-Null
$checkResult = dotnet $csharpierDllPath Check.cs --check
$checkResult = $checkResult -join "`n"
if ($LASTEXITCODE -ne 1) {
    Write-Output "The exit code from --check was $($LASTEXITCODE) but was expected to be 1"
    Write-Output ""
    $failed = $true
}
Remove-Item "Check.cs"

Write-Output "---- Check should print unformatted file"
$wasNotFormatted = "Was not formatted"
if (-not ($checkResult.Contains($wasNotFormatted))) {
    Write-Output "The result from --check did not contain '$($wasNotFormatted)', it was: "
    Write-Output $checkResult
    Write-Output ""
    $failed = $true
}

Write-Output "---- Basic file should be formatted"
New-Item -Path . -Name "Code.cs" -Value $unformatted 2>&1 | Out-Null
dotnet $csharpierDllPath Code.cs 2>&1 | Out-Null
$codeContents = Get-Content "Code.cs"
if ($codeContents -ne $formatted) {
    Write-Output "The result of formatting a basic class was"
    Write-Output $codeContents
    Write-Output "Expected"
    Write-Output $formatted
    Write-Output ""
    $failed = $true
}
Remove-Item "Code.cs"

Write-Output "---- Ignore file respected when directoryOrFile is '.' and csharpierignore has subdirectory"
New-Item -Path "Subdirectory" -ItemType "Directory" | Out-Null
New-Item -Path "Subdirectory" -Name "IgnoredFile.cs" -Value $unformatted | Out-Null
New-Item -Path . -Name ".csharpierignore" -Value "Subdirectory/IgnoredFile.cs" | Out-Null
dotnet $csharpierDllPath . 2>&1 | Out-Null
$ignoredFileContents = Get-Content "Subdirectory/IgnoredFile.cs"
if ($ignoredFileContents -ne $unformatted) {
    Write-Output "The file at Subdirectory/IgnoredFile.cs should have been ignored but it was formatted"
    Write-Output ""
    $failed = $true
}
Remove-Item "Subdirectory" -Recurse -Force
Remove-Item ".csharpierignore" -Force

Write-Output "---- DirectoryOrFile is required when not using stdin"
$noDirectoryResult = dotnet $csharpierDllPath 2>&1 | Out-Null
$noDirectoryResult = $noDirectoryResult -join "`n"
$missingDirectoryOrFailes = "directoryOrFile is required when not piping stdin to CSharpier"
if (-not ($missingDirectoryOrFailes.Contains($missingDirectoryOrFailes))) {
    Write-Output "The result from running with no options did not contain '$($missingDirectoryOrFailes)', it was: "
    Write-Output $missingDirectoryOrFailes
    Write-Output ""
    $failed = $true
}

if ($failed) {
    exit 1;
}
