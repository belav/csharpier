FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

COPY 

COPY nupkg ./nupkg
COPY Src/CSharpier.MsBuild.Test ./Src/CSharpier.MsBuild.Test

WORKDIR Src/CSharpier.MsBuild.Test

RUN dotnet build -c Release
