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

$failureMessage = ""

foreach ($scenario in $scenarios) {
    Write-Host "::group::$($scenario.name)"

    $scenarioPath = Join-Path $basePath $scenario.name
    New-Item $scenarioPath -ItemType Directory | Out-Null

    $dockerFile = Join-Path $scenarioPath "DockerFile"

    Set-Content -Path $dockerFile -Value "FROM $($scenario.sdk)
WORKDIR /app
COPY ./nupkg ./nupkg
COPY ./nuget.config ./nuget.config
COPY ./GeneratedScenarios/$($scenario.name)/Project.csproj ./
RUN dotnet build -c Release
"


    $csprojFile = Join-Path $scenarioPath "Project.csproj"

    $csharpierFrameworkVersion = ""
    if ($null -ne $scenario.csharpier_frameworkVersion) {
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
    <PackageReference Include=`"CSharpier-Config.MsBuild`" Version=`"0.0.1`">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
</Project>
"

    docker build . -f $dockerFile

    if ($LASTEXITCODE -ne 0) {
        $failureMessage += "::error::The scenario $($scenario.name) failed to build. See the logs above for details`n"
    }

    Write-Host "::endgroup::"
}

if ($failureMessage -ne "") {
    Write-Host $failureMessage
    exit 1
}

