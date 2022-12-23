---
title: MsBuild Package
hide_table_of_contents: true
---

CSharpier can be run when a project is built by installing the CSharpier.MSBuild nuget package
```console
Install-Package CSharpier.MSBuild
```

## Target Frameworks
By default CSharpier.MsBuild will run CSharpier using the target framework of the project it is running on.  
This can be controlled with the following property. This property is required if the csproj is targeting < net6.0 (netstandard2.0, net48, etc)
```xml
  <PropertyGroup>
    <CSharpier_FrameworkVersion>net6.0</CSharpier_FrameworkVersion>
  </PropertyGroup>
```



By default this will 
- In Debug - on build will run `dotnet csharpier` in the project folder to format all files in the project.
- In Release - on build will run `dotnet csharpier --check` in the project folder to validate that all files in the project have already been formatted.

You can control when `--check` is used with the following Property
```xml
  <PropertyGroup>
    <CSharpier_Check>false</CSharpier_Check>
  </PropertyGroup>
```
