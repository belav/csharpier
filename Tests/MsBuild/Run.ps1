Set-Location $PSScriptRoot

Write-Host "::group::Build CSharpier.MsBuild::"

dotnet pack ../../Src/CSharpier.MsBuild/CSharpier.MsBuild.csproj -o nupkg /p:Version=0.0.1

Write-Host "::endgroup::"

$scenarios = Get-Content Scenarios.json | ConvertFrom-JSON

$basePath = "./GeneratedScenarios"

if (Test-Path $basePath) {
    Remove-Item $basePath -Recurse -Force
}

New-Item $basePath -ItemType Directory | Out-Null

$failureMessages = @()

foreach ($scenario in $scenarios) {
    # these fail on windows in GH and because they use docker for the scenarios we don't need to run them twice anyway
    if ($env:GithubOS -eq "windows-latest") {
        continue
    }
    
    Write-Host "::group::$($scenario.name)"

    $scenarioPath = Join-Path $basePath $scenario.name
    Write-Host $scenarioPath
    New-Item $scenarioPath -ItemType Directory | Out-Null

    $dockerFile = Join-Path $scenarioPath "DockerFile"

    Set-Content -Path $dockerFile -Value "FROM $($scenario.sdk)
WORKDIR /app
COPY ./nupkg ./nupkg
COPY ./nuget.config ./nuget.config
COPY ./.csharpierignore ./.csharpierignore
COPY ./GeneratedScenarios/$($scenario.name)/Project.csproj ./
RUN dotnet build -c Release
"


    $csprojFile = Join-Path $scenarioPath "Project.csproj"

    $csharpierFrameworkVersion = ""
    if ([bool]($scenario.PSobject.Properties.name -match "csharpier_frameworkVersion")) {
        $csharpierFrameworkVersion = "
    <CSharpier_FrameworkVersion>$($scenario.csharpier_frameworkVersion)</CSharpier_FrameworkVersion>
"
    }

    Set-Content -Path $csprojFile -Value "<Project Sdk=`"Microsoft.NET.Sdk`">
  <PropertyGroup>
    <TargetFrameworks>$($scenario.targetFrameworks)</TargetFrameworks>
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
        $failureMessages += "The scenario $($scenario.name) failed to build. See the logs above for details"
    }

    Write-Host "::endgroup::"
}


Write-Host "::group::UnformattedFileCausesError"
$output = [TestHelper]::RunTestCase("UnformattedFileCausesError", $true)
Write-Host "::endgroup::"

Write-Host "::group::FileThatCantCompileCausesOneError"
$output = [TestHelper]::RunTestCase("FileThatCantCompileCausesOneError", $true)
if (-not($output.Contains("1 Error(s)"))) {
    $failureMessages += "The TestCase FileThatCantCompileCausesOneError did not contain the text '1 Error(s)1"
}

Write-Host "::endgroup::"

if ($failureMessages.Length -ne 0) {
    foreach ($message in $failureMessages) {
        Write-Host "::error::$message`n"
    }
    exit 1
}

class TestHelper {
    static [string] RunTestCase([string] $testCase, [bool] $expectErrorCode) {
        $output = (& dotnet build -c Release ./TestCases/$($testCase)/Project.csproj) | Out-String
        Write-Host $output
        
        $expectedExitCode = 0
        if ($expectErrorCode -eq $true) {
            $expectedExitCode = 1
        }

        if ($LASTEXITCODE -ne $expectedExitCode) {
            $failureMessages += "The TestCase $testCase did not return an exit code of $expectedExitCode"
        }
        
        return $output
    }
}

