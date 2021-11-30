## Editor Integration
### Visual Studio
Check out [WillFuqua.RunOnSave](https://marketplace.visualstudio.com/items?itemName=WillFuqua.RunOnSave)
### Visual Studio Code
Use the [official extension](https://marketplace.visualstudio.com/items?itemName=csharpier.csharpier-vscode)


### Rider
1. Open Settings
2. Tools - File Watchers
3. Add New File Watcher
   * File Type: C# File
   * Program: dotnet
   * Arguments: csharpier $FilePath$
   * Output paths to refresh: $FilePath$
   * Advanced Options - Auto-save edited files...: This should probably be off otherwise if you pause while coding csharpier will reformat the file as is.

## Continuous Integration
Normally when using a code formatter like CSharpier, you'll want to ensure that all code that makes it to your main branch has been formatted. This can be accomplished by doing the following
1. Set up a dotnet tool manifest file at `.config/dotnet-tools.json` with the following content. Replacing `[SpecificVersion]` with the version of CSharpier you are currently using. 
   ```json
   {
       "version": 1,
       "isRoot": true,
       "tools": {
           "csharpier": {
              "version": "[SpecificVersion]",
              "commands": [
                  "dotnet-csharpier"
              ]
          }
       }
   }
   ```
2. Use your preferred CI/CD tool to run the following commands.  
   ```bash
   dotnet tool restore
   dotnet-csharpier --check
   ```   
   An example of a github action to accomplish this
   ```yaml
   name: Validate PR
   on:
     pull_request:
       branches: [ master ]
   jobs:
     check_formatting:
       runs-on: ubuntu-latest
       name: Check Formatting
       steps:
         - uses: actions/checkout@v2
         - run: |
           dotnet tool restore
           dotnet csharpier --check
   
   ```

