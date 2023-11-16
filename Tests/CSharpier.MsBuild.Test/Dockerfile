FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

COPY ./nupkg ./nupkg
COPY ./Tests/CSharpier.MsBuild.Test ./Tests/CSharpier.MsBuild.Test

RUN dotnet build Tests/CSharpier.MsBuild.Test -c Release
