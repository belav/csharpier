---
title: MsBuild Package
hide_table_of_contents: true
---

CSharpier can be run when a project is built by installing the CSharpier.MSBuild nuget package
```console
Install-Package CSharpier.MSBuild
```

By default this will 
- In Debug - on build will run `dotnet csharpier` in the project folder to format all files in the project.
- In Release - on build will run `dotnet csharpier --check` in the project folder to validate that all files in the project have already been formatted.

## Properties

### Check

You can control when `--check` is used with the following Property
```xml
  <PropertyGroup>
    <CSharpier_Check>false</CSharpier_Check>
  </PropertyGroup>
```

### Bypass

You can bypass running CSharpier using the `CSharpier_Bypass` Property
```xml
  <PropertyGroup>
    <CSharpier_Bypass>true</CSharpier_Bypass>
  </PropertyGroup>
```

```bash
dotnet publish -c release -o /app --no-restore /p:CSharpier_Bypass=true
```

### Log Level
You can control the value passed to `--loglevel` with the following Property
```xml
  <PropertyGroup>
    <CSharpier_LogLevel>Error</CSharpier_LogLevel>
  </PropertyGroup>
```
Valid options are:
- None
- Error
- Warning
- Information (default)
- Debug

### Target Frameworks
CSharpier.MSBuild will be run with net6.0, net7.0 or net8.0 if the project targets one of the three frameworks. In cases where the project targets something else (net48, netstandard2.0) `CSharpier_FrameworkVersion` will default to net7.0
This can be controlled with the following property. This property is required if the csproj is targeting < net6.0 (netstandard2.0, net48, etc) and net7.0 is not installed.
```xml
  <PropertyGroup>
    <CSharpier_FrameworkVersion>net6.0</CSharpier_FrameworkVersion>
  </PropertyGroup>
```
