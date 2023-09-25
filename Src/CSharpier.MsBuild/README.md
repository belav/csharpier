One way to test the changes in the build/* files
- Load CSharpier.MsBuild.csproj in your IDE
- Restore the nuget packages
- Edit those files at `C:\Users\[Username]\.nuget\packages\csharpier.msbuild\[VersionNumber]\build`
- Ensure you revert those files and make the same changes to the files here.

Another way to test

- Making any changes you want
```powershell
Import-Module (RepoRoot)/Shell/Init.ps1
CSH-BuildPackages
```
- change the version in CSharpier.MSBuild.Test.csproj (until we update to 0.25.0 or higher), then it can be $(Version)
- Running this from the root `docker build . -f ./Src/CSharpier.MsBuild.Test/Dockerfile`

