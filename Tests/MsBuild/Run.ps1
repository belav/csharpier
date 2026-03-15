param(
    [switch]$SkipScenarios
)

Set-Location $PSScriptRoot

Write-Host "::group::Build CSharpier.MsBuild::"

# if we don't clear this when running locally, then the projects below keep pulling in the same 0.0.1 package
dotnet nuget locals --clear all

dotnet pack ../../Src/CSharpier.MsBuild/CSharpier.MsBuild.csproj -o nupkg /p:Version=0.0.1

Write-Host "::endgroup::"

$scenarios = Get-Content Scenarios.json | ConvertFrom-JSON

$basePath = "./GeneratedScenarios"

if (Test-Path $basePath) {
    Remove-Item $basePath -Recurse -Force
}

New-Item $basePath -ItemType Directory | Out-Null

$failureMessages = @()

if (-not $SkipScenarios) {
    foreach ($scenario in $scenarios) {
        # these fail on windows in GH and because they use docker for the scenarios we don't need to run them twice anyway
        if ($env:GithubOS -eq "windows-latest") {
            continue
        }

        Write-Host "::group::$( $scenario.name )"

        $scenarioPath = Join-Path $basePath $scenario.name
        Write-Host $scenarioPath
        New-Item $scenarioPath -ItemType Directory | Out-Null

        $dockerFile = Join-Path $scenarioPath "DockerFile"

        Set-Content -Path $dockerFile -Value "FROM $( $scenario.sdk )
WORKDIR /app
COPY ./nupkg ./nupkg
COPY ./nuget.config ./nuget.config
COPY ./not_csharpierignore ./.csharpierignore
COPY ./GeneratedScenarios/$( $scenario.name )/Project.csproj ./
RUN dotnet build -c Release
"


        $csprojFile = Join-Path $scenarioPath "Project.csproj"

        $csharpierFrameworkVersion = ""
        if ([bool]($scenario.PSobject.Properties.name -match "csharpier_frameworkVersion")) {
            $csharpierFrameworkVersion = "
    <CSharpier_FrameworkVersion>$( $scenario.csharpier_frameworkVersion )</CSharpier_FrameworkVersion>
"
        }

        Set-Content -Path $csprojFile -Value "<Project Sdk=`"Microsoft.NET.Sdk`">
  <PropertyGroup>
    <TargetFrameworks>$( $scenario.targetFrameworks )</TargetFrameworks>
    $csharpierFrameworkVersion
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=`"CSharpier.MsBuild`" Version=`"0.0.1`">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
</Project>
"

        docker build . -f $dockerFile

        if ($LASTEXITCODE -ne 0) {
            $failureMessages += "The scenario $( $scenario.name ) failed to build. See the logs above for details"
        }

        Write-Host "::endgroup::"
    }
}

# we don't want the actual csharpier run to find the unformatted files in these projects
# so they are ignored in the root .csharpierignore
# but that would mean our test projects never try to format them unless we create this
# empty ignore file. We delete it at the end
New-Item .csharpierignore -force

# I hate powershell, ugly hacks to get output to return and also update the running list of failures
Write-Host "::group::UnformattedFileCausesError"
# TODO these no longer fail the build
$result = [TestHelper]::RunTestCase("UnformattedFileCausesError", $true)
if ($result.Length -gt 1) {
    $failureMessages += $result[1]
}
Write-Host "::endgroup::"

Write-Host "::group::UnformattedFileCausesWarning"
$result = [TestHelper]::RunTestCase("UnformattedFileCausesWarning", $false)
if ($result.Length -gt 1) {
    $failureMessages += $result[1]
}
Write-Host "::endgroup::"

Write-Host "::group::LogLevel"
$result = [TestHelper]::RunTestCase("LogLevel", $true)
if ($result.Length -gt 1) {
    $failureMessages += $result[1]
}
Write-Host "::endgroup::"

Write-Host "::group::FileThatCantCompileCausesOneError"
$result = [TestHelper]::RunTestCase("FileThatCantCompileCausesOneError", $true)
if ($result.Length -gt 1) {
    $failureMessages += $result[1]
}
if (-not($result[0].Contains("1 Error(s)"))) {
    $failureMessages += "The TestCase FileThatCantCompileCausesOneError did not contain the text '1 Error(s)1"
}

Write-Host "::endgroup::"

# TODO need a test to validate that a file is actually formatted to help prevent breaking these again

if ($failureMessages.Length -ne 0) {
    foreach ($message in $failureMessages) {
        Write-Host "::error::$message`n"
    }
    exit 1
}

class TestHelper {
    static [string[]] RunTestCase([string] $testCase, [bool] $expectErrorCode) {
        $output = (& dotnet build -c Release ./TestCases/$($testCase)/Project.csproj) | Out-String
        # because we can't disable gh showing annotations on these files -- https://github.com/actions/toolkit/issues/457
        # replace error with something GH doesn't look for which essentially disables those annotations
        Write-Host $output.Replace("error ", "érror ")
        
        $expectedExitCode = 0
        if ($expectErrorCode -eq $true) {
            $expectedExitCode = 1
        }

        $result = @();
        $result += $output

        if ($LASTEXITCODE -ne $expectedExitCode) {

            $result += "The TestCase $testCase did not return an exit code of $expectedExitCode"
        }
        
        return $result
    }
}

Remove-Item .csharpierignore

