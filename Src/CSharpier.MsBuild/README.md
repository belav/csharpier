One way to test the changes in the build/* files
- Load CSharpier.MsBuild.csproj in your IDE
- Restore the nuget packages
- Edit those files at `C:\Users\[Username]\.nuget\packages\csharpier.msbuild\[VersionNumber]\build`
- Ensure you revert those files and make the same changes to the files here.

Some automated tests exist
- the validate PR GH action runs these, mostly around framework versions
- cd ./Tests/MsBuild
- ./Run.ps1

Other things that would be really really nice to automate
- exits properly in release when no files formatted  
  https://github.com/belav/csharpier/issues/1357
- same as above if thing set
- formats files in debug
- formats files if told to in release
- a few scenarios in here I think, compilation errors etc  
  https://github.com/belav/csharpier/issues/1131 
