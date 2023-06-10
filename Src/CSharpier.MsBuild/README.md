This can be tested by

- Making any changes you want
```powershell
Import-Module (RepoRoot)/Shell/Init.ps1
CSH-BuildPackages
```
- Running this from the root `docker build . -f ./Src/CSharpier.MsBuild.Test/Dockerfile`

