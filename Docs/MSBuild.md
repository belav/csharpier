### MSBuild Package

**CSharpier.MSBuild is not currently compatible with `dotnet watch run` see [#228](https://github.com/belav/csharpier/issues/228)**

CSharpier can be run when a package is built by installing the CSharpier.MSBuild nuget package
```console
Install-Package CSharpier.MSBuild
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
