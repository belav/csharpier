One way to test the changes in the build/* files
- Load CSharpier.MsBuild.csproj in your IDE
- Restore the nuget packages
- Edit those files at `C:\Users\[Username]\.nuget\packages\csharpier.msbuild\[VersionNumber]\build`
- Ensure you revert those files and make the same changes to the files here.

Some automated tests exist
- the validate PR GH action runs these
- cd ./Tests/MsBuild
- ./Run.ps1 - some of these don't seem to work well locally

Other things that would be really really nice to automate
- formats files in debug
- formats files if told to in release
- checks files if told to in debug
- log levels