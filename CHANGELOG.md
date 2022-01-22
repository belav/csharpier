# 0.14.0

[diff](https://github.com/belav/csharpier/compare/0.13.0...0.14.0)

- File with no preprocessor symbols formats twice. [#555](https://github.com/belav/csharpier/issues/555)
- A namespace with `assembly` attribute and `using` causes two newlines [#551](https://github.com/belav/csharpier/issues/551)
- Wrapping arithmetic expressions not stacked/chopped down [#547](https://github.com/belav/csharpier/issues/547)
- Use UTF8 for piping in/out to CLI to support unicode characters  [#545](https://github.com/belav/csharpier/issues/545)


# 0.13.0

[diff](https://github.com/belav/csharpier/compare/0.12.0...0.13.0)

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

# 0.12.0

[diff](https://github.com/belav/csharpier/compare/0.11.1...0.12.0)

### Breaking Changes
- If a file that fails to compile is piped to csharpier, csharpier now writes an error message on std error and return a 1 exit code.  

### Changes
- Nested Initializers should break [#487](https://github.com/belav/csharpier/issues/487)
- Adding --pipe-multiple-files and other changes to support vscode extension [#283](https://github.com/belav/csharpier/pull/495)


# 0.11.2 (CSharpier.MSBuild only)
- CSharpier.MSBuild 0.11.1 is not published correctly [#481](https://github.com/belav/csharpier/issues/481)

# 0.11.1

[diff](https://github.com/belav/csharpier/compare/0.10.0...0.11.1)

- base should merge in an invocation chain [#473](https://github.com/belav/csharpier/issues/473)
- File with multiple newlines at the end keeps them [#464](https://github.com/belav/csharpier/issues/464)
- Extra space in generic with omitted types [#463](https://github.com/belav/csharpier/issues/463)
- Object creation still uses SpaceBrace [#462](https://github.com/belav/csharpier/issues/462)
- Empty Initializer gets double whitespace [#461](https://github.com/belav/csharpier/issues/461)
- Support C# 10 and .Net 6 [#448](https://github.com/belav/csharpier/issues/448)
- Always break nested Conditionals  [#434](https://github.com/belav/csharpier/issues/434)


# 0.10.0

[diff](https://github.com/belav/csharpier/compare/0.9.9...0.10.0)

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


# 0.9.9

[diff](https://github.com/belav/csharpier/compare/0.9.8...0.9.9)

### Breaking Changes

- Require the directoryOrFile argument when not piping into to stdin [#381](https://github.com/belav/csharpier/issues/381)

### Changes
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


# 0.9.8

[diff](https://github.com/belav/csharpier/compare/0.9.7...0.9.8)

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

# 0.9.7

[diff](https://github.com/belav/csharpier/compare/0.9.6...0.9.7)

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

# 0.9.6

[diff](https://github.com/belav/csharpier/compare/0.9.5...0.9.6)

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
______
# 0.9.5

[diff](https://github.com/belav/csharpier/compare/0.9.4...0.9.5)

- CSharpier.MSBuild does not work with dotnet watch run [#228](https://github.com/belav/csharpier/issues/228)
- Leading comments interfering with breaking InitializerExpression [#217](https://github.com/belav/csharpier/issues/217)
- \#endregion is getting indented more on each format [#216](https://github.com/belav/csharpier/issues/216)
- Some files getting extra new lines on each format [#215](https://github.com/belav/csharpier/issues/215)
- File that fails check should give some indication of the formatting that was missing. [#182](https://github.com/belav/csharpier/issues/182)
- Missing nodes in SyntaxNodeComparer need better reporting. [#160](https://github.com/belav/csharpier/issues/160)

# 0.9.4

[diff](https://github.com/belav/csharpier/compare/0.9.3...0.9.4)

- Interpolated verbatim string is not handling line endings [#221](https://github.com/belav/csharpier/issues/221)
- CLI Support for multiple targets [#220](https://github.com/belav/csharpier/issues/220)
- Add support for nuget package that inject msbuild step to run csharpier [#218](https://github.com/belav/csharpier/issues/218)
- Loops without braces [#202](https://github.com/belav/csharpier/issues/202)

# 0.9.3

[diff](https://github.com/belav/csharpier/compare/0.9.2...0.9.3)

- RecordDeclaration with Generics produces invalid code [#201](https://github.com/belav/csharpier/issues/201)
- Formatting of auto implemented properties with access modifiers [#188](https://github.com/belav/csharpier/issues/188)
- Verbatim string literals take into account EndOfLine configuration [#183](https://github.com/belav/csharpier/issues/183)
- CSharpierIgnore & CSharpierRC from parent directories should be respected. [#181](https://github.com/belav/csharpier/issues/181)
- Break apart readme [#172](https://github.com/belav/csharpier/issues/172)
- PatternMatching includes extra spaces [#167](https://github.com/belav/csharpier/issues/167)
- Re-add async file reads [#127](https://github.com/belav/csharpier/issues/127)
- Dictionary Initializer inserts extra new line [#103](https://github.com/belav/csharpier/issues/103)

#0.9.2
[diff](https://github.com/belav/csharpier/compare/0.9.1...0.9.2)

- Support "auto" for endOfLine [#147](https://github.com/belav/csharpier/issues/147)
- Long Parameter Attribute should break after ending brace [#174](https://github.com/belav/csharpier/issues/174)
- Attribute on parameter should have space [#104](https://github.com/belav/csharpier/issues/104)

# 0.9.1

[diff](https://github.com/belav/csharpier/compare/0.9.0...0.9.1)

- Add support for .csharpierignore [#159](https://github.com/belav/csharpier/issues/159)
- Break before BinaryOperator [#152](https://github.com/belav/csharpier/issues/152)
- LeadingComments affect breaking [#149](https://github.com/belav/csharpier/issues/149)
- Ignore generated files by default [#140](https://github.com/belav/csharpier/issues/140)
- Return with BinaryExpression [#137](https://github.com/belav/csharpier/issues/137)
- IsPattern breaking inside of IfStatement [#130](https://github.com/belav/csharpier/issues/130)
- SpaceBrace causing breaking when it shouldn't [#100](https://github.com/belav/csharpier/issues/100)
- Implement Formatting Options with Configuration File [#10](https://github.com/belav/csharpier/issues/10)
