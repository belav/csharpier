Set-Location $PSScriptRoot

dotnet pack ../../Src/CSharpier.MsBuild/CSharpier.MsBuild.csproj -o nupkg /p:Version=0.0.1

$scenarios = Get-Content Scenarios.json | ConvertFrom-JSON

$basePath = "./GeneratedScenarios"

if (Test-Path $basePath) {
    Remove-Item $basePath -Recurse -Force
}

New-Item $basePath -ItemType Directory | Out-Null

$failureMessage = ""

Write-Host "::group::Running Scenarios"

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

    Set-Content -Path $csprojFile -Value "<Project Sdk=`"Microsoft.NET.Sdk`">
  <PropertyGroup>
    <TargetFrameworks>$($scenario.targetFrameworks)</TargetFrameworks>
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
        $failureMessage += "::error::The scenario $($scenario.name) failed to build. See the logs above for details`n"
    }

    Write-Host "::endgroup::"
}

Write-Host "::endgroup::"

if ($failureMessage -ne "") {
    Write-Host $failureMessage
    exit 1
}

