One way to test the changes in the build/* files
- Load CSharpier.MsBuild.csproj in your IDE
- Restore the nuget packages
- Edit those files at `C:\Users\[Username]\.nuget\packages\csharpier.msbuild\[VersionNumber]\build`
- Ensure you revert those files and make the same changes to the files here.

Another way
- the validate PR GH action does this, currently only uses the net8 sdk
- dotnet pack Src/CSharpier.MsBuild/CSharpier.MsBuild.csproj -o nupkg /p:Version=0.0.1
- docker build -f ./Tests/CSharpier.MsBuild.Test/Dockerfile .
