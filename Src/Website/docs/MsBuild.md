---
title: MsBuild Package
hide_table_of_contents: true
---

CSharpier can be run when a package is built by installing the CSharpier.MSBuild nuget package
```console
Install-Package CSharpier.MSBuild
```

As of 0.15.0 This requires .NET6 to be installed. Previous versions require .NET5

By default this will 
- In Debug - on build will run `dotnet csharpier` in the project folder to format all files in the project.
- In Release - on build will run `dotnet csharpier --check` in the project folder to validate that all files in the project have already been formatted.

You can control when `--check` is used with the following Property
```xml
  <PropertyGroup>
    <CSharpier_Check>false</CSharpier_Check>
  </PropertyGroup>
```
