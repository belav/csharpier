---
title: Continuous Integration
hide_table_of_contents: true
---

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
   dotnet csharpier --check .
   ```   
   An example Github Actions workflow to accomplish this
   ```yaml
   name: Validate PR
   on:
     pull_request:
       branches: [ main ]
   jobs:
     check_formatting:
       runs-on: ubuntu-latest
       name: Check Formatting
       steps:
         - uses: actions/checkout@v2
         - run: |
             dotnet tool restore
             dotnet csharpier --check .
   
   ```

