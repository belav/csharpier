$csharpier = $PSScriptRoot + "/../Src/CSharpier/bin/release/net5.0/dotnet-csharpier.dll"
if (-not (Test-Path $csharpier)) {
    Write-Output "No dll found at $($csharpier)" 
    exit 1
}

$formatted = "public class ClassName { }"
$unformatted = "public class ClassName  { }"

$failed = $false;

$stdInResult = $unformatted | dotnet $csharpier
if ($stdInResult -ne $formatted) {
    Write-Output "Piping c# code to CSharpier did not result in formatted code. The result was:"
    Write-Output $stdInResult
    Write-Output ""
    $failed = $true;
}

New-Item -Path . -Name "Check.cs" -Value $unformatted
$checkResult = dotnet $csharpier Check.cs --check
$checkResult = $checkResult -join "`n"
if ($LASTEXITCODE -ne 1) {
    Write-Output "The exit code from --check was $($LASTEXITCODE) but was expected to be 1"
    Write-Output ""
    $failed = $true
}

$wasNotFormatted = "was not formatted"
if (-not ($checkResult.Contains($wasNotFormatted))) {
    Write-Output "The result from --check did not contain '$(wasNotFormatted)', it was: "
    Write-Output $checkResult
    Write-Output ""
    $failed = $true
}

New-Item -Path . -Name "Code.cs" -Value $unformatted
dotnet $csharpier Code.cs
$formattedResult = Get-Content "Code.cs"
if ($formattedResult -ne $formatted) {
    Write-Output "The result of formatting a basic class was"
    Write-Output $formattedResult
    Write-Output "Expected"
    Write-Output $formatted
    Write-Output ""
}

$noDirectoryResult = dotnet $csharpier
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
