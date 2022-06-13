# 0.18.0
## What's Changed
#### Initial C# 11 support [#686](https://github.com/belav/csharpier/pull/686)
CSharpier can format the following c# 11 features
- Raw string literals
- Generic attributes
- Static abstract members in interfaces
- Newlines in string interpolation expressions **CSharpier will leave existing new lines within expressions and not add new ones**
- List Patterns
- UTF8 string literals
- Unsigned right shift operator
- Checked operator
- Generic math

#### use relative file path in CommandLineFormatter [#680](https://github.com/belav/csharpier/pull/680)
CSharpier now outputs relative or absolute file paths so that they are clickable in terminals.
```
dotnet csharpier .

# csharpier 0.17.0
Error Invalid.cs - Failed to compile so was not formatted.

# csharpier 0.18.0
Error ./Invalid.cs - Failed to compile so was not formatted.

dotnet csharpier c:/src

# csharpier 0.17.0
Error Invalid.cs - Failed to compile so was not formatted.

# csharpier 0.18.0
Error c:/src/Invalid.cs - Failed to compile so was not formatted.
```

Thanks go to @dlech

#### Invalid code for comments inside expressions in verbatim interpolated strings [#679](https://github.com/belav/csharpier/issues/679)
```c#
// input
var someValue =
    $@"
    {
        // comment
        "hi"
    }
    ";
// 0.17.0
var someValue =
    $@"
    {
        // comment "hi"}
    ";
// 0.18.0
var someValue =
    $@"
    {
        // comment
        "hi"
    }
    ";
```
Thanks go to @ivan-razorenov
#### CSharpier ranged ignore [#678](https://github.com/belav/csharpier/issues/678)
CSharpier now has the ability to ignore a range of statements or members. See [Ignore](https://csharpier.com/docs/Ignore) for more details
```c#
// csharpier-ignore-start
var unformatted =        true;
var unformatted =        true;
// csharpier-ignore-end
```
Thanks go to @pingzing

**Full Changelog**: https://github.com/belav/csharpier/compare/0.17.0...0.18.0


# 0.17.0
## What's Changed

- MSBuild Task target too late? Breakpoints are not hit [#674](https://github.com/belav/csharpier/issues/674)
- Excessive indent level with lambda as the only method call argument [#669](https://github.com/belav/csharpier/issues/669)
- Empty (or malformed) .csproj file will cause csharpier to fail. [#665](https://github.com/belav/csharpier/issues/665)
- #endif retains extra blank lines [#660](https://github.com/belav/csharpier/issues/660)
- Option for indentation  [#645](https://github.com/belav/csharpier/issues/645)
- Small bug with formatting LINQ queries with multiple orderby fields [#643](https://github.com/belav/csharpier/issues/643)
- Consistently Indent By 4 Spaces [#617](https://github.com/belav/csharpier/issues/617)
- Conditional access edge cases [#603](https://github.com/belav/csharpier/issues/603)
- Improve formatting for casting [#407](https://github.com/belav/csharpier/issues/407)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.16.0...0.17.0

# 0.16.0
## What's Changed

- fix: ignore file detection when directory contains period [#634](https://github.com/belav/csharpier/pull/634)
- Format switch statement consistently with other code. [#624](https://github.com/belav/csharpier/pull/624)
- CodeFormatter should accept SyntaxTree [#621](https://github.com/belav/csharpier/issues/621)
- Add support for netstandard 2.0 to CSharpier.Core [#619](https://github.com/belav/csharpier/pull/619)
- Indent c style multiline comments correctly when they switch indentation. [#606](https://github.com/belav/csharpier/issues/606)
- Member access should break [#600](https://github.com/belav/csharpier/issues/600)
- SuppressNullableWarningExpression ( !. ) does not break consistenly [#596](https://github.com/belav/csharpier/issues/596)
- Turn CSharpier.com into a proper website. [#505](https://github.com/belav/csharpier/issues/505)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.15.1...0.16.0

# 0.15.1
## What's Changed
- Set CSharpier.MsBuild as DevelopmentDependency. [#607](https://github.com/belav/csharpier/pull/607)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.15.0...0.15.1
 
# 0.15.0
## Breaking Changes
- `CSharpier.MsBuild` now requires .NET6 [#565](https://github.com/belav/csharpier/issues/565)

## What's Changed
- .csharpierignore causes csharpier to be significantly slower [#594](https://github.com/belav/csharpier/issues/594)
- Support for // csharpier-ignore [#581](https://github.com/belav/csharpier/issues/581)
- Multiline comments are not properly indented. [#580](https://github.com/belav/csharpier/issues/580)
- Generics + ObjectCreationExpression should break consistently [#578](https://github.com/belav/csharpier/issues/578)
- Extra blank lines should be removed at the end of a method [#575](https://github.com/belav/csharpier/issues/575)
- Null conditional operator does not break consistently [#561](https://github.com/belav/csharpier/issues/561)
- Enum members should follow the rules for new lines [#553](https://github.com/belav/csharpier/issues/553)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.14.0...0.15.0

# 0.14.0
## What's Changed
- File with no preprocessor symbols formats twice. [#555](https://github.com/belav/csharpier/issues/555)
- A namespace with `assembly` attribute and `using` causes two newlines [#551](https://github.com/belav/csharpier/issues/551)
- Wrapping arithmetic expressions not stacked/chopped down [#547](https://github.com/belav/csharpier/issues/547)
- Use UTF8 for piping in/out to CLI to support unicode characters  [#545](https://github.com/belav/csharpier/issues/545)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.13.0...0.14.0

# 0.13.0
## What's Changed
- Incorrect indentation in Method chain inside Initializer [#529](https://github.com/belav/csharpier/issues/529)
- Allow empty lines before comments in enums [#524](https://github.com/belav/csharpier/issues/524)
- For with empty statement should have space. [#523](https://github.com/belav/csharpier/issues/523)
- Empty lines are not respected with break and continue [#520](https://github.com/belav/csharpier/issues/520)
- Extra Whitespace in empty anonymous initializer [#519](https://github.com/belav/csharpier/issues/519)
- Class that ends with comment does not retain extra line before comment [#513](https://github.com/belav/csharpier/issues/513)
- Join Clause with Type losing Type [#508](https://github.com/belav/csharpier/issues/508)
- Give cli bad path, you get an exception [#506](https://github.com/belav/csharpier/issues/506)
- Double blank lines appearing in top level statements [#501](https://github.com/belav/csharpier/issues/501)
- VisualStudio Extension [#499](https://github.com/belav/csharpier/issues/499)
- Rider Plugin [#498](https://github.com/belav/csharpier/issues/498)
- CSharpier.MSBuild may have mismatched version with CLI [#490](https://github.com/belav/csharpier/issues/490)
- Break object initializers if there are 3 or more properties [#446](https://github.com/belav/csharpier/issues/446)
- Force lines before and after some members. [#285](https://github.com/belav/csharpier/issues/285)
- Formatting may conflict with StyleCopAnalayzers [#13](https://github.com/belav/csharpier/issues/13)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.12.0...0.13.0

# 0.12.0
## Breaking Changes
- If a file that fails to compile is piped to csharpier, csharpier now writes an error message on std error and return a 1 exit code.  

## What's Changed
- Nested Initializers should break [#487](https://github.com/belav/csharpier/issues/487)
- Adding --pipe-multiple-files and other changes to support vscode extension [#283](https://github.com/belav/csharpier/pull/495)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.11.1...0.12.0

# 0.11.2 (CSharpier.MSBuild only)
## What's Changed
- CSharpier.MSBuild 0.11.1 is not published correctly [#481](https://github.com/belav/csharpier/issues/481)

# 0.11.1
## What's Changed
- base should merge in an invocation chain [#473](https://github.com/belav/csharpier/issues/473)
- File with multiple newlines at the end keeps them [#464](https://github.com/belav/csharpier/issues/464)
- Extra space in generic with omitted types [#463](https://github.com/belav/csharpier/issues/463)
- Object creation still uses SpaceBrace [#462](https://github.com/belav/csharpier/issues/462)
- Empty Initializer gets double whitespace [#461](https://github.com/belav/csharpier/issues/461)
- Support C# 10 and .Net 6 [#448](https://github.com/belav/csharpier/issues/448)
- Always break nested Conditionals  [#434](https://github.com/belav/csharpier/issues/434)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.10.0...0.11.1

# 0.10.0
## What's Changed
- try-finally without catch clause is formatted strangely. [#454](https://github.com/belav/csharpier/issues/454)
- Nested FixedStatements should break [#438](https://github.com/belav/csharpier/issues/438)
- Disabled text validation fails with trailing whitespace [#428](https://github.com/belav/csharpier/issues/428)
- Vertically Align Curly Braces [#423](https://github.com/belav/csharpier/issues/423)
- Crash On Empty Config File [#421](https://github.com/belav/csharpier/issues/421)
- Conditional in Arguments should indent. [#419](https://github.com/belav/csharpier/issues/419)
- Chained assignments formatting can be improved [#417](https://github.com/belav/csharpier/issues/417)
- Improve ConditionalExpression in ReturnStatement formatting [#416](https://github.com/belav/csharpier/issues/416)
- Pattern Matching edge cases [#413](https://github.com/belav/csharpier/issues/413)
- Implement proper logging. [#406](https://github.com/belav/csharpier/issues/406)
- (finally) Improve formatting of InvocationExpressions [#7](https://github.com/belav/csharpier/issues/7)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.9...0.10.0

# 0.9.9
## Breaking Changes
- Require the directoryOrFile argument when not piping into to stdin [#381](https://github.com/belav/csharpier/issues/381)

## What's Changed
- SwitchExpression + Pattern edge cause causes extra line and poor formatting [#408](https://github.com/belav/csharpier/issues/408)
- NewLines not retained before lock statement [#401](https://github.com/belav/csharpier/issues/401)
- Better error handling when given a csproj or sln file [#398](https://github.com/belav/csharpier/issues/398)
- CSharpierignore not taken into account when supplying . as the directory [#392](https://github.com/belav/csharpier/issues/392)
- SwitchStatement with When breaks even with body of switch [#387](https://github.com/belav/csharpier/issues/387)
- Respect new lines between case statements [#383](https://github.com/belav/csharpier/issues/383)
- Line breaks in "when" clause in SwitchExpression [#382](https://github.com/belav/csharpier/issues/382)
- Block loses extra lines [#378](https://github.com/belav/csharpier/issues/378)
- RecordDeclaration should format consistently with ConstructorDeclaration [#371](https://github.com/belav/csharpier/issues/371)
- Conditional breaking without indentation [#345](https://github.com/belav/csharpier/issues/345)
- Improve formatting of pattern matching (IsPatternExpression, BinaryPattern, etc) [#154](https://github.com/belav/csharpier/issues/154)
- Code in IfDirective can't currently be formatted [#15](https://github.com/belav/csharpier/issues/15)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.8...0.9.9

# 0.9.8
## What's Changed
- Remove all configuration options except for print width. [#358](https://github.com/belav/csharpier/issues/358)
- Array Rank not breaking [#353](https://github.com/belav/csharpier/issues/353)
- SwitchStatement should SpaceBrace [#352](https://github.com/belav/csharpier/issues/352)
- ObjectInitializer keeping brace on same line [#336](https://github.com/belav/csharpier/issues/336)
- ObjectInitializer in CollectionInitializer breaking [#335](https://github.com/belav/csharpier/issues/335)
- Better format do-while [#317](https://github.com/belav/csharpier/issues/317)
- Blocks inside of other blocks are getting an extra new line. [#316](https://github.com/belav/csharpier/issues/316)
- Implicit Object Creation breaking  [#302](https://github.com/belav/csharpier/issues/302)
- ForEachVariableStatement not breaking/indenting consistently with regular ForEachStatement [#300](https://github.com/belav/csharpier/issues/300)
- MethodDeclaration with Constraints not breaking before brace [#299](https://github.com/belav/csharpier/issues/299)
- Constructor with Base edge cases [#298](https://github.com/belav/csharpier/issues/298)
- Nested Generics in Variable Declaration [#295](https://github.com/belav/csharpier/issues/295)
- #pragma or trailing comment causes breaking in Object Initializer [#252](https://github.com/belav/csharpier/issues/252)
- Verbatim string with mismatched line endings triggers "failed syntax tree validation" [#244](https://github.com/belav/csharpier/issues/244)
- SwitchExpression formatting. [#237](https://github.com/belav/csharpier/issues/237)
- Empty Method should keep braces on same line [#133](https://github.com/belav/csharpier/issues/133)
- Improving formatting for edge cases of ForStatement [#112](https://github.com/belav/csharpier/issues/112)
- ConditionalExpression indentation [#83](https://github.com/belav/csharpier/issues/83)
- BinaryExpression Grouping [#37](https://github.com/belav/csharpier/issues/37)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.7...0.9.8

# 0.9.7
## What's Changed
- New overload for `Doc.GroupWithId()` [#334](https://github.com/belav/csharpier/issues/334)
- Improve formatting by grouping parenthesized expressions and indenting them if they break. [#328](https://github.com/belav/csharpier/issues/328)
- Improve formatting of IsPattern in IfStatement [#327](https://github.com/belav/csharpier/issues/327)
- Improve formatting of the condition in a do-while [#326](https://github.com/belav/csharpier/issues/326)
- Always break statements without braces. [#303](https://github.com/belav/csharpier/issues/303)
- Empty Line being added with Array Type [#301](https://github.com/belav/csharpier/issues/301)
- Implicit Array Initializer does not format braces consistently with other statements. [#297](https://github.com/belav/csharpier/issues/297)
- Format checked like a invocation expression with an argument list [#270](https://github.com/belav/csharpier/issues/270)
- Attribute should newline before close paren [#257](https://github.com/belav/csharpier/issues/257)
- Tuple with VariableDeclaration [#251](https://github.com/belav/csharpier/issues/251)
- Record - does not format consistently with a class. [#233](https://github.com/belav/csharpier/issues/233)
- CSharpier.Playground should only publish with new released version [#224](https://github.com/belav/csharpier/issues/224)
- Attributes on parameters [#204](https://github.com/belav/csharpier/issues/204)
- Improve Lambda Formatting - indent expression body and break in a way to minimize new lines. [#176](https://github.com/belav/csharpier/issues/176)
- Format ObjectCreationExpression with Initializer consistently [#113](https://github.com/belav/csharpier/issues/113)
- Improve formatting of long Catch Clauses [#86](https://github.com/belav/csharpier/issues/86)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.6...0.9.7

# 0.9.6
## What's Changed
- Add options to write the formatted file to stdout and accept a file from stdin [#282](https://github.com/belav/csharpier/issues/282)
- Implement ConditionalGroup doc type [#278](https://github.com/belav/csharpier/issues/278)
- Optimize some hot paths to speed up formatting. [#277](https://github.com/belav/csharpier/issues/277)
- Implement Align Doc Type [#276](https://github.com/belav/csharpier/issues/276)
- Improve formatting of ClassDeclaration with BaseList + Constraints [#275](https://github.com/belav/csharpier/issues/275)
- Switch tests to width 100 so they line up with default option [#256](https://github.com/belav/csharpier/issues/256)
- Improving formatting of generics + variable declarations. [#240](https://github.com/belav/csharpier/pull/240)
- Improve Forrmatting of Field with lambda and generics [#236](https://github.com/belav/csharpier/issues/236)
- Improve Formatting of object initialiser syntax [#234](https://github.com/belav/csharpier/issues/234)
- Improve formatting of generic methods and constructors [#94](https://github.com/belav/csharpier/issues/94)
- Improve formatting of field with generics [#47](https://github.com/belav/csharpier/issues/47)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.5...0.9.6

# 0.9.5
## What's Changed
- CSharpier.MSBuild does not work with dotnet watch run [#228](https://github.com/belav/csharpier/issues/228)
- Leading comments interfering with breaking InitializerExpression [#217](https://github.com/belav/csharpier/issues/217)
- \#endregion is getting indented more on each format [#216](https://github.com/belav/csharpier/issues/216)
- Some files getting extra new lines on each format [#215](https://github.com/belav/csharpier/issues/215)
- File that fails check should give some indication of the formatting that was missing. [#182](https://github.com/belav/csharpier/issues/182)
- Missing nodes in SyntaxNodeComparer need better reporting. [#160](https://github.com/belav/csharpier/issues/160)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.4...0.9.5

# 0.9.4
## What's Changed
- Interpolated verbatim string is not handling line endings [#221](https://github.com/belav/csharpier/issues/221)
- CLI Support for multiple targets [#220](https://github.com/belav/csharpier/issues/220)
- Add support for nuget package that inject msbuild step to run csharpier [#218](https://github.com/belav/csharpier/issues/218)
- Loops without braces [#202](https://github.com/belav/csharpier/issues/202)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.3...0.9.4

# 0.9.3
## What's Changed
- RecordDeclaration with Generics produces invalid code [#201](https://github.com/belav/csharpier/issues/201)
- Formatting of auto implemented properties with access modifiers [#188](https://github.com/belav/csharpier/issues/188)
- Verbatim string literals take into account EndOfLine configuration [#183](https://github.com/belav/csharpier/issues/183)
- CSharpierIgnore & CSharpierRC from parent directories should be respected. [#181](https://github.com/belav/csharpier/issues/181)
- Break apart readme [#172](https://github.com/belav/csharpier/issues/172)
- PatternMatching includes extra spaces [#167](https://github.com/belav/csharpier/issues/167)
- Re-add async file reads [#127](https://github.com/belav/csharpier/issues/127)
- Dictionary Initializer inserts extra new line [#103](https://github.com/belav/csharpier/issues/103)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.2...0.9.3

#0.9.2
## What's Changed
- Support "auto" for endOfLine [#147](https://github.com/belav/csharpier/issues/147)
- Long Parameter Attribute should break after ending brace [#174](https://github.com/belav/csharpier/issues/174)
- Attribute on parameter should have space [#104](https://github.com/belav/csharpier/issues/104)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.1...0.9.2

# 0.9.1
## What's Changed
- Add support for .csharpierignore [#159](https://github.com/belav/csharpier/issues/159)
- Break before BinaryOperator [#152](https://github.com/belav/csharpier/issues/152)
- LeadingComments affect breaking [#149](https://github.com/belav/csharpier/issues/149)
- Ignore generated files by default [#140](https://github.com/belav/csharpier/issues/140)
- Return with BinaryExpression [#137](https://github.com/belav/csharpier/issues/137)
- IsPattern breaking inside of IfStatement [#130](https://github.com/belav/csharpier/issues/130)
- SpaceBrace causing breaking when it shouldn't [#100](https://github.com/belav/csharpier/issues/100)
- Implement Formatting Options with Configuration File [#10](https://github.com/belav/csharpier/issues/10)

**Full Changelog**: https://github.com/belav/csharpier/compare/0.9.0...0.9.1
