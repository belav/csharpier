This can be tested by

- Making any changes you want
```powershell
Import-Module (RepoRoot)/Shell/Init.ps1
CSH-BuildPackages
```
- change the version in CSharpier.MSBuild.Test.csproj (until we update to 0.25.0 or higher), then it can be $(Version)
- Running this from the root `docker build . -f ./Src/CSharpier.MsBuild.Test/Dockerfile`

