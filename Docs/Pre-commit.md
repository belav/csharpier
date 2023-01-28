---
title: Pre-commit Hook
hide_table_of_contents: true
---

CSharpier can be used with a pre-commit hook to ensure that all staged files are formatted before being committed.

## [pre-commit](https://pre-commit.com)

Add the following to your `.pre-commit-config.yaml`:

```yaml
repos:
    - repo: https://github.com/belav/csharpier
      rev: 0.23.0 # Replace with the latest Git tag.
      hooks:
        - id: csharpier
```

[Install pre-commit via your preferred Python package manager.](https://pre-commit.com/#install)
[Run `pre-commit install` to install the Git hook scripts.](https://pre-commit.com/#3-install-the-git-hook-scripts)

## [Husky.Net](https://github.com/alirezanet/husky.net)

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

Once you are sure the task is working properly, you can add it as a pre-commit hook.
```bash
dotnet husky add pre-commit -c "dotnet husky run"
```

If you want the pre-commit hook to be opt in, ignore the `.husky/pre-commit` file. It can be enabled by individual developers if the run the command above.
