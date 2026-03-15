---
title: Pre-commit Hook
hide_table_of_contents: true
---

CSharpier can be used with a pre-commit hook to ensure that all staged files are formatted before being committed.

## [pre-commit](https://pre-commit.com)

[Install pre-commit via your preferred Python package manager.](https://pre-commit.com/#install)
[Run `pre-commit install` to install the Git hook scripts.](https://pre-commit.com/#3-install-the-git-hook-scripts)


### [MegaLinter](https://megalinter.io/)

CSharpier runs as part of
[MegaLinter's pre-commit hooks](https://megalinter.io/latest/mega-linter-runner/#pre-commit-hook).


### Standalone

If you prefer not to run the other linters included in MegaLinter, you can
alternatively run CSharpier as a local pre-commit hook.

Add the following to your `.pre-commit-config.yaml`:

```yaml
repos:
  - repo: local
    hooks:
      - id: dotnet-tool-restore
        name: Install .NET tools
        entry: dotnet tool restore
        # for pre-commit < 4.4.0 use language: system
        language: unsupported
        always_run: true
        pass_filenames: false
        stages:
          - commit
          - push
          - post-checkout
          - post-rewrite
        description: Install the .NET tools listed at .config/dotnet-tools.json.
      - id: csharpier
        name: Run CSharpier on C# files
        entry: dotnet tool run CSharpier format
        language: system
        types:
          - c#
        description: CSharpier is an opinionated C# formatter inspired by Prettier.
```

## [Husky.Net](https://github.com/alirezanet/husky.net)

From the root of your repository
```bash
cd <Your project root directory>
dotnet new tool-manifest
dotnet tool install husky
dotnet husky install
```

Modify the file at `.husky/task-runner.json`. Include any file extensions that you want to be formatted by CSharpier.
```json
{
    "tasks": [{
        "name": "Run csharpier",
        "command": "dotnet",
        "args": [ "csharpier", "format", "${staged}" ],
        "include": [
            "**/*.cs",
            "**/*.csx",
            "**/*.csproj",
            "**/*.props",
            "**/*.targets",
            "**/*.xml",
            "**/*.config"
        ]
    }]
}
```

You can run and test your task with the following command.
```bash
dotnet husky run
```

Optionally - add this to one of your projects to automate the installation for future developers
You can set the HUSKY environment variable to 0 to disable Husky in CI/CD pipelines.
```bash
dotnet husky attach <path-to-project-file>
```

Once you are sure the task is working properly, you can add it as a pre-commit hook.
```bash
dotnet husky add pre-commit -c "dotnet husky run"
```

If you want the pre-commit hook to be opt in, ignore the `.husky/pre-commit` file. It can be enabled by individual developers if the run the command above.
