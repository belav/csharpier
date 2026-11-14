## 1. Invariant tests (write first, expect failures)

- [x] 1.1 Add `CompareFullSpanTests` asserting equality for identical text sliced from two *distinct* string instances, equality at a shifted offset, and inequality for differing text. Build a non-interned second string (e.g. via `StringBuilder`) — two identical literals are interned to the same instance and would make the test pass against the broken code.
- [x] 1.2 Add an XML formatter test asserting `CodeFormatterResult.AST` is empty when `PrinterOptions.IncludeAST` is false and populated when true.
- [x] 1.3 Add a source-level test scanning `CSharpier.Core` for `new DocListBuilder(` and asserting every occurrence is bound by a `using` declaration. Follow the existing `CSharpier.Tests/MissingTypeChecker.cs` precedent for source/reflection-driven checks.
- [x] 1.4 Add a `DocListBuilder` unit test asserting that after `Dispose()` the internal span is empty, so a use-after-dispose read fails rather than silently reading a returned buffer.
- [x] 1.5 Add a lambda-body print-count test: format an invocation with a single simple-lambda argument and assert the body printer runs exactly once; extend it to nested single-lambda arguments and assert the count grows linearly, not exponentially.
- [x] 1.6 Run the new tests and confirm 1.1, 1.3 and 1.5 **fail** against current `main`. If any passes, the test is not exercising the defect — fix the test before proceeding.

## 2. Restore the two broken optimizations

- [x] 2.1 Change `CSharp/SyntaxNodeComparer.cs:383` from `originalSpan == formattedSpan` to `originalSpan.SequenceEqual(formattedSpan)`.
- [x] 2.2 Change `CSharpier.Benchmarks/Program.cs:79` to construct `SyntaxNodeComparer` with two distinct-but-equal strings instead of `(this.code, this.code)`, so the fast path is exercised in the shape production actually uses.
- [x] 2.3 Gate `Xml/XmlFormatter.cs:52` on `printerOptions.IncludeAST`, matching `CSharpFormatter.cs:133` and `:190`.
- [x] 2.4 Confirm tests 1.1 and 1.2 now pass.

## 3. Return pooled buffers

- [x] 3.1 In `Utilities/DocListBuilder.cs`, clear `this.span` in `Dispose()` alongside nulling `arrayFromPool`.
- [x] 3.2 Convert the 21 non-disposing `new DocListBuilder(` sites to `using var`. Sites: `SyntaxPrinter/AttributeLists.cs:22`; `SyntaxNodePrinters/` — `AnonymousMethodExpression.cs:11`, `AnonymousObjectMemberDeclarator.cs:14`, `Argument.cs:21`, `AttributeList.cs:16`, `BaseFieldDeclaration.cs:11`, `BasePropertyDeclaration.cs:118`, `DelegateDeclaration.cs:11`, `EnumMemberDeclaration.cs:11`, `IfStatement.cs:11`, `Interpolation.cs:11`, `InvocationExpression.cs:388`, `LabeledStatement.cs:11`, `Parameter.cs:14`, `QueryBody.cs:11`, `RecursivePattern.cs:25`, `SwitchSection.cs:11`, `TryStatement.cs:11`, `UsingStatement.cs:12`, `VariableDeclarator.cs:11`; and `Xml/XNodePrinters/Node.cs:23`.
- [x] 3.3 Where the final element count is known up front rather than built conditionally, replace the builder with a right-sized `new Doc[n]` — it ties the disposed builder on bytes and avoids the pool round-trip entirely. Evaluate `AttributeLists.cs`, `Argument.cs` and `Parameter.cs` for this; leave the rest on the builder.
- [x] 3.4 In `SyntaxNodePrinters/Argument.cs:19`, add an early return before constructing the builder for the common case where the argument has neither `NameColon` nor a `RefKindKeyword` — today it rents a buffer per argument of every method call only to return `Doc.Null`.
- [x] 3.5 Confirm tests 1.3 and 1.4 now pass.

## 4. Stop recomputing known values

- [x] 4.1 Add `public int PrintedWidth { get; }` to `DocTypes/StringDoc.cs`, computed eagerly in the constructor via `value.GetPrintedWidth()`. Verify `StringDoc` does not grow — the `int` should occupy the padding beside `bool IsDirective`.
- [x] 4.2 Read `stringDoc.PrintedWidth` at `DocPrinter/DocPrinter.cs:276` and `DocPrinter/DocFitter.cs:61` instead of calling `GetPrintedWidth()`.
- [x] 4.3 Add a width test covering ASCII, East Asian wide characters and mixed content, asserting `PrintedWidth` matches the previous per-character calculation exactly.
- [x] 4.4 Guard the walk in `CSharp/PreprocessorSymbols.cs` on `syntaxTree.GetRoot().ContainsDirectives`, hoisted into the static `GetSets(SyntaxTree)` so the walker and its collections are never constructed for a directive-free file.
- [x] 4.5 Add tests asserting a directive-free file returns an empty symbol-set list, and that a file with `#if`/`#elif`/`#else` produces the same sets as before.

## 5. Token fast paths (behavioral risk — verify byte-exactness)

- [x] 5.1 In `Token.cs:425 PrintTrailingTrivia`, scan the trivia list for a comment kind before renting the builder; return `Doc.Null` when there is none. Hoist the doubled `trivia.RawSyntaxKind()` at `:428`/`:432` into a local.
- [x] 5.2 In `Token.cs:258 PrivatePrintLeadingTrivia`, add a pre-scan that returns `Doc.Null` before allocating the `List<Doc>` when the trivia is only whitespace and end-of-line. The guard MUST also require `!includeInitialNewLines` and `!context.State.NextTriviaNeedsLine` — with either set, an otherwise-empty list still emits a doc. When in any doubt, fall through to the existing path.
- [x] 5.3 In `Token.cs:58 PrintSyntaxToken`, compute the leading and trailing trivia docs up front and short-circuit to `StringDoc.Create(syntaxToken)` when both are `Doc.Null`, there is no suffix, and the kind is not one of the multi-line string kinds. Hoist `syntaxToken.RawSyntaxKind()` — it is currently evaluated at `:53`, `:71`, `:75`, `:88` and `:129`.
- [x] 5.4 Run the full `dotnet test` suite. Any expected-output difference means a guard condition is wrong — fix the condition, do not update the expected output.

## 6. Share the lambda body doc (behavioral risk — land separately)

- [x] 6.1 In `SyntaxPrinter/ArgumentListLikeSyntax.cs:35-36`, hoist `SimpleLambdaExpression.PrintBody(simpleLambda, context)` into a local and use it in both branches of the `Doc.IfBreak`, matching the sibling parenthesized-lambda branch at `:70`.
- [x] 6.2 Run the full `dotnet test` suite, paying particular attention to trailing-comma, `csharpier-ignore` and modifier-reordering cases — `PrintBody` mutates `CSharpPrintingContext.State`, so calling it once instead of twice is a behavioral change.
- [x] 6.3 Confirm test 1.5 now passes. If any expected-output test moves, revert this task group alone; the rest of the change stands without it.

## 7. Verify and measure

- [x] 7.1 Run the full `dotnet test` suite and confirm every expected-output comparison passes unchanged.
- [x] 7.2 Run `dotnet run -c Release --project Src/CSharpier.Benchmarks` and record `Default_CodeFormatter_Tests` and `Default_CodeFormatter_Complex` against the pre-change baseline of 59.4 ms / 27.6 MB and 128.5 ms / 47.5 MB.
- [x] 7.3 Confirm allocated bytes per format dropped. If they did not, the `using var` conversion missed sites — re-run the 1.3 source scan.
- [x] 7.4 Record the measured before/after in the change notes so the next audit has a baseline to compare against.
