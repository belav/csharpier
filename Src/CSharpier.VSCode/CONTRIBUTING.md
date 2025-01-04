Local Testing
- open this folder with vscode
- npm install
- npm run start (this will watch and recompile any changes you make)
- in vscode you can use Run - Start Debugging or Run - Run Without Debugging
- After you are running the extension in vscode you can use ctrl r to reload window (Developer: Reload Window) when you make changes to the extension.

Publishing
- Update version in package.json
- run `npx vsce package`
- run `npx vsce publish -p <token>` an old one is stored and I haven't been able to unstore it
- run 'npx ovsx publish -p <token>'

https://marketplace.visualstudio.com/manage/publishers/csharpier
