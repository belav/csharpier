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


### StyleCopAnalyzers
[StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) is a set of analyzers
that implement StyleCop rules. This is the known set of rules that conflict with CSharpier.
There are probably additional rules that are unnecessary because CSharpier formats code in a way that satisfies the rule.

```xml
<Rule Id="SA1000" Action="None" />
<Rule Id="SA1009" Action="None" />
<Rule Id="SA1111" Action="None" />
<Rule Id="SA1118" Action="None" />

<!-- see https://github.com/belav/csharpier/issues/527 -->
<Rule Id="SA1127" Action="None" />

<!-- see https://github.com/belav/csharpier/issues/526 -->
<Rule Id="SA1128" Action="None" />
<Rule Id="SA1137" Action="None" />
<Rule Id="SA1500" Action="None" />
<Rule Id="SA1501" Action="None" />
<Rule Id="SA1502" Action="None" />
<Rule Id="SA1504" Action="None" />

<!-- This could be left on. It tries to force a new line before a comment for a catch. The comment can be moved
 to the end of the line for the closing brace of the try -->
<Rule Id="SA1513" Action="None" />
<Rule Id="SA1516" Action="None" />
```
