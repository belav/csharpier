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