Local Testing
- I have run into cases of the extension not working. Uninstalling both csharpier and 
csharpier 2019 from the test instance and restarting seems to fix it. Maybe it happens because both seem to
to installed? Turning off the 2019 build for debug seems to have helped

Publishing
- Update version in BOTH files at CSharpier.VisualStudio[2019]/source.extension.vsixmanifest
- build solution in release (can this happen via command line?)
- go to https://marketplace.visualstudio.com/manage/publishers/csharpier (with personal microsoft account)
- three dots - edit - add new vsix
- Automate this if we release a lot
