# running this seems to screw up the nuget restore, but provides a way to figure out why a test is failing on linux while working on windows.
# you have to run this from the root, IE powershell ./Scripts/RunLinuxTests.ps1
docker run --rm -v ${pwd}:/app -e "NormalizeLineEndings=1" -w /app/tests mcr.microsoft.com/dotnet/sdk:5.0 dotnet test /app/Src/CSharpier.Tests/CSharpier.Tests.csproj --logger:trx

# gross way to run csharpier against the csharpier-repos
#docker run --rm -v ${pwd}:/app -e "NormalizeLineEndings=1" -w /app mcr.microsoft.com/dotnet/sdk:5.0 dotnet ./csharpier/Src/CSharpier/bin/Debug/net5.0/dotnet-csharpier.dll csharpier-repos --skip-write
