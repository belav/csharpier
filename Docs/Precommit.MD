## Precommit Hooks

CSharpier can be used with a pre-commit hook to ensure that all staged files are formatted before being committed.

## [Husky.Net](https://github.com/alirezanet/husky.net)
Husky.Net makes setting up a precommit hook simple. 

From the root of your repository
```bash
dotnet tool install husky
dotnet husky install
```

Optionally - add this to one of your projects to automate the install for future developers
```xml
<!-- set HUSKY to 0 in CI/CD disable this -->
<Target Name="husky" BeforeTargets="Restore;CollectPackageReferences" Condition="'$(HUSKY)' != 0">
    <Exec Command="dotnet tool restore"  StandardOutputImportance="Low" StandardErrorImportance="High"/>
    <Exec Command="dotnet husky install" StandardOutputImportance="Low" StandardErrorImportance="High"
        <!-- update this to be the root of your solution --> 
        WorkingDirectory="../../" />
</Target>
```

Modify the file at `.husky/task-runner.json`
```json
{
    "tasks": [{
        "name": "Run csharpier",
        "command": "dotnet",
        "args": [ "csharpier", "${staged}" ],
        "include": [ "**/*.cs" ]
    }]
}
```

You can run and test your task with the following command.
```bash
dotnet husky run
```

Once you are sure the task is working properly, you can add it as a precommit hook.
```bash
dotnet husky add pre-commit -c "dotnet husky run"
```
