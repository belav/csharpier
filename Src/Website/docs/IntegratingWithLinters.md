---
title: Integrating with Linters
hide_table_of_contents: true
---


More often than not, linters contain formatting rules that conflict with CSharpier.

### Dotnet Format
[Dotnet Format](https://github.com/dotnet/format) provides command line arguments to run different subsets of rules. Running the following will skip the whitespace formatting rules and avoid conflicts with CSharpier
```bash
# Prior to .Net6
dotnet-format --fix-style info --fix-analyzers info

# .Net6
dotnet format style
dotnet format analyzers
```

### Code Analysis Rules
A large number of formatting rules are all under a single rule ID IDE0055. A number of the options for it conflict with CSharpier.  
Disabling the rule is recommended for now. See [#781](https://github.com/belav/csharpier/issues/781) for more information.
```editorconfig
dotnet_diagnostic.IDE0055.severity = none
```




### StyleCopAnalyzers
[StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) is a set of analyzers
that implement StyleCop rules. This is the known set of rules that conflict with CSharpier.

```editorconfig
dotnet_diagnostic.SA1000.severity = none
dotnet_diagnostic.SA1009.severity = none
dotnet_diagnostic.SA1111.severity = none
dotnet_diagnostic.SA1118.severity = none
dotnet_diagnostic.SA1137.severity = none
dotnet_diagnostic.SA1500.severity = none
dotnet_diagnostic.SA1501.severity = none
dotnet_diagnostic.SA1502.severity = none
dotnet_diagnostic.SA1504.severity = none
dotnet_diagnostic.SA1516.severity = none
                                    
# will be changed with https://github.com/belav/csharpier/issues/527
dotnet_diagnostic.SA1127.severity = none
# will be changed with https://github.com/belav/csharpier/issues/526
dotnet_diagnostic.SA1128.severity = none
```

There are additional rules that can be disabled because they are not needed when using CSharpier
```editorconfig
dotnet_diagnostic.SA1001.severity = none
dotnet_diagnostic.SA1002.severity = none
dotnet_diagnostic.SA1003.severity = none
dotnet_diagnostic.SA1007.severity = none
dotnet_diagnostic.SA1008.severity = none
dotnet_diagnostic.SA1010.severity = none
dotnet_diagnostic.SA1011.severity = none
dotnet_diagnostic.SA1012.severity = none
dotnet_diagnostic.SA1013.severity = none
dotnet_diagnostic.SA1014.severity = none
dotnet_diagnostic.SA1015.severity = none
dotnet_diagnostic.SA1016.severity = none
dotnet_diagnostic.SA1017.severity = none
dotnet_diagnostic.SA1018.severity = none
dotnet_diagnostic.SA1019.severity = none
dotnet_diagnostic.SA1020.severity = none
dotnet_diagnostic.SA1021.severity = none
dotnet_diagnostic.SA1022.severity = none
dotnet_diagnostic.SA1023.severity = none
dotnet_diagnostic.SA1024.severity = none
dotnet_diagnostic.SA1025.severity = none
dotnet_diagnostic.SA1026.severity = none
dotnet_diagnostic.SA1027.severity = none
dotnet_diagnostic.SA1028.severity = none
dotnet_diagnostic.SA1102.severity = none
dotnet_diagnostic.SA1103.severity = none
dotnet_diagnostic.SA1104.severity = none
dotnet_diagnostic.SA1105.severity = none
dotnet_diagnostic.SA1107.severity = none
dotnet_diagnostic.SA1110.severity = none
dotnet_diagnostic.SA1112.severity = none
dotnet_diagnostic.SA1113.severity = none
dotnet_diagnostic.SA1114.severity = none
dotnet_diagnostic.SA1115.severity = none
dotnet_diagnostic.SA1116.severity = none
dotnet_diagnostic.SA1117.severity = none
dotnet_diagnostic.SA1136.severity = none
dotnet_diagnostic.SA1505.severity = none
dotnet_diagnostic.SA1506.severity = none
dotnet_diagnostic.SA1507.severity = none
dotnet_diagnostic.SA1508.severity = none
dotnet_diagnostic.SA1509.severity = none
dotnet_diagnostic.SA1510.severity = none
dotnet_diagnostic.SA1511.severity = none
dotnet_diagnostic.SA1517.severity = none
dotnet_diagnostic.SA1518.severity = none
```
