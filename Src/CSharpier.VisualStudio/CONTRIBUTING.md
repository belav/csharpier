Local Testing
- Open this project in VisualStudio
- Use VS to start the extension with/without debugging
- I have run into cases of the extension not working, it seems to sometimes install the 2022 and 2019 version into the instance of VS used to test  
Uninstalling both csharpier and csharpier 2019 from the test instance and restarting seems to fix it. 

Publishing
- Update version in BOTH files at CSharpier.VisualStudio[2019]/source.extension.vsixmanifest
- Update ChangeLog.md
- build solution in release (can this happen via command line?)
- use cli
  - CSH-PublishVS [AccessToken] 
- old way
  - go to https://marketplace.visualstudio.com/manage/publishers/csharpier (with personal microsoft account)
  - three dots - edit - add new vsix
