FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base

COPY nupkg ./nupkg
COPY Src/CSharpier.MsBuild.Test ./Src/CSharpier.MsBuild.Test

WORKDIR Src/CSharpier.MsBuild.Test

RUN dotnet build -c Release
