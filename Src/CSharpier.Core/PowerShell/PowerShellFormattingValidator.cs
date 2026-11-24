using System.Collections;
using System.Collections.Concurrent;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CSharpier.Core.PowerShell;

// Parses the original and the formatted text and walks both trees side by side. Everything the
// parser records about a node except its position has to match, and so do the comments. Keyword
// casing, operator casing, statement separators, line continuations, and whitespace are not part
// of the tree, so changing them passes.
internal class PowerShellFormattingValidator(string originalCode, string formattedCode)
    : IFormattingValidator
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesByType = new();

    public Task<FormattingValidatorResult> ValidateAsync(CancellationToken cancellationToken)
    {
        return LargeStack.Run(
            Math.Max(originalCode.Length, formattedCode.Length),
            () =>
            {
                try
                {
                    var failureMessage = this.Validate(cancellationToken);
                    return new FormattingValidatorResult
                    {
                        Failed = failureMessage is not null,
                        FailureMessage = failureMessage,
                    };
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    return new FormattingValidatorResult
                    {
                        Failed = true,
                        FailureMessage = "Failed with ex " + ex,
                    };
                }
            },
            cancellationToken
        );
    }

    private string? Validate(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var originalAst = Parser.ParseInput(
            originalCode,
            out var originalTokens,
            out var originalErrors
        );

        cancellationToken.ThrowIfCancellationRequested();
        var formattedAst = Parser.ParseInput(
            formattedCode,
            out var formattedTokens,
            out var formattedErrors
        );

        var originalBlockingErrors = originalErrors.Count(PowerShellParseErrors.IsBlocking);
        var formattedBlockingErrors = formattedErrors
            .Where(PowerShellParseErrors.IsBlocking)
            .ToArray();
        if (formattedBlockingErrors.Length > originalBlockingErrors)
        {
            var error = formattedBlockingErrors[0];
            return $"The formatted code has a parse error at line {error.Extent.StartLineNumber}: {error.Message}";
        }

        var originalComments = GetComments(originalTokens);
        var formattedComments = GetComments(formattedTokens);
        for (
            var index = 0;
            index < Math.Max(originalComments.Count, formattedComments.Count);
            index++
        )
        {
            var original = index < originalComments.Count ? originalComments[index] : null;
            var formatted = index < formattedComments.Count ? formattedComments[index] : null;
            if (original is null)
            {
                return $"The formatted code has an extra comment at line {formatted!.Extent.StartLineNumber}:\n{formatted.Text}";
            }

            if (formatted is null)
            {
                return $"The formatted code is missing the comment from line {original.Extent.StartLineNumber} of the original:\n{original.Text}";
            }

            if (NormalizeComment(original.Text) != NormalizeComment(formatted.Text))
            {
                return $"The comment at line {original.Extent.StartLineNumber} of the original does not match the comment at line {formatted.Extent.StartLineNumber} of the formatted code.\n"
                    + $"Original:\n{original.Text}\nFormatted:\n{formatted.Text}";
            }
        }

        return CompareTrees(originalAst, formattedAst, cancellationToken);
    }

    private static List<Token> GetComments(Token[] tokens)
    {
        return tokens.Where(o => o.Kind == TokenKind.Comment).ToList();
    }

    private static string NormalizeComment(string comment)
    {
        return string.Join("\n", comment.Replace("\r\n", "\n").Split('\n').Select(o => o.Trim()));
    }

    private readonly record struct Pair(
        object? Original,
        object? Formatted,
        Ast OriginalOwner,
        Ast FormattedOwner,
        string PropertyName
    );

    // Walks both trees with an explicit stack, so a deeply nested tree such as a long operator
    // chain cannot overflow the stack. Children are pushed in reverse so they are compared in order.
    private static string? CompareTrees(
        Ast originalRoot,
        Ast formattedRoot,
        CancellationToken cancellationToken
    )
    {
        var pending = new Stack<Pair>();
        pending.Push(new Pair(originalRoot, formattedRoot, originalRoot, formattedRoot, "Root"));
        while (pending.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = CompareValues(pending.Pop(), pending);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    private static string? CompareValues(Pair pair, Stack<Pair> pending)
    {
        var (originalValue, formattedValue, originalOwner, formattedOwner, propertyName) = pair;
        if (originalValue is null || formattedValue is null)
        {
            return originalValue is null && formattedValue is null
                ? null
                : Describe(originalOwner, formattedOwner, $"{propertyName} was added or removed");
        }

        switch (originalValue)
        {
            case Ast originalAst:
                if (formattedValue is not Ast formattedAst)
                {
                    return Describe(originalOwner, formattedOwner, $"{propertyName} changed");
                }

                if (originalAst.GetType() != formattedAst.GetType())
                {
                    return Describe(
                        originalAst,
                        formattedAst,
                        $"the node type changed from {originalAst.GetType().Name} to {formattedAst.GetType().Name}"
                    );
                }

                var properties = GetProperties(originalAst.GetType());
                for (var index = properties.Length - 1; index >= 0; index--)
                {
                    var property = properties[index];
                    pending.Push(
                        new Pair(
                            property.GetValue(originalAst),
                            property.GetValue(formattedAst),
                            originalAst,
                            formattedAst,
                            property.Name
                        )
                    );
                }

                return null;
            case string originalString:
                return
                    formattedValue is string formattedString
                    && (
                        NormalizeLineEndings(originalString)
                            == NormalizeLineEndings(formattedString)
                        || (
                            IsStopParsingArgument(originalOwner)
                            && originalString.TrimEnd() == formattedString
                        )
                    )
                    ? null
                    : Describe(
                        originalOwner,
                        formattedOwner,
                        $"{propertyName} changed from \"{originalValue}\" to \"{formattedValue}\""
                    );
            case ITypeName originalTypeName:
                return
                    formattedValue is ITypeName formattedTypeName
                    && originalTypeName.FullName == formattedTypeName.FullName
                    ? null
                    : Describe(originalOwner, formattedOwner, $"{propertyName} changed");
            case ITuple originalTuple:
                if (
                    formattedValue is not ITuple formattedTuple
                    || originalTuple.Length != formattedTuple.Length
                )
                {
                    return Describe(originalOwner, formattedOwner, $"{propertyName} changed");
                }

                for (var index = originalTuple.Length - 1; index >= 0; index--)
                {
                    pending.Push(
                        pair with
                        {
                            Original = originalTuple[index],
                            Formatted = formattedTuple[index],
                        }
                    );
                }

                return null;
            case IEnumerable originalEnumerable:
                if (formattedValue is not IEnumerable formattedEnumerable)
                {
                    return Describe(originalOwner, formattedOwner, $"{propertyName} changed");
                }

                var originalItems = originalEnumerable.Cast<object?>().ToList();
                var formattedItems = formattedEnumerable.Cast<object?>().ToList();
                if (originalItems.Count != formattedItems.Count)
                {
                    return Describe(
                        originalOwner,
                        formattedOwner,
                        $"{propertyName} had {originalItems.Count} items and now has {formattedItems.Count}"
                    );
                }

                for (var index = originalItems.Count - 1; index >= 0; index--)
                {
                    pending.Push(
                        pair with
                        {
                            Original = originalItems[index],
                            Formatted = formattedItems[index],
                        }
                    );
                }

                return null;
            default:
                return
                    originalValue.Equals(formattedValue)
                    || (
                        originalValue.GetType() == formattedValue.GetType()
                        && originalValue.ToString() == formattedValue.ToString()
                    )
                    ? null
                    : Describe(
                        originalOwner,
                        formattedOwner,
                        $"{propertyName} changed from \"{originalValue}\" to \"{formattedValue}\""
                    );
        }
    }

    // The argument after --% runs to the end of the line, and trailing whitespace is not kept.
    private static bool IsStopParsingArgument(Ast node)
    {
        return node.Parent is CommandAst command
            && command.CommandElements.Count > 1
            && command.CommandElements[^1] == node
            && command.CommandElements[^2].Extent.Text == "--%";
    }

    private static PropertyInfo[] GetProperties(Type type)
    {
        return PropertiesByType.GetOrAdd(type, static type =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(o =>
                    o.GetIndexParameters().Length == 0
                    && o.Name != nameof(Ast.Parent)
                    && !typeof(IScriptExtent).IsAssignableFrom(o.PropertyType)
                    && !typeof(Type).IsAssignableFrom(o.PropertyType)
                    && o.PropertyType != typeof(ScriptRequirements)
                )
                .ToArray()
        );
    }

    private static string NormalizeLineEndings(string value)
    {
        return value.Replace("\r\n", "\n");
    }

    private static string Describe(Ast original, Ast formatted, string difference)
    {
        return $"In {original.GetType().Name}, {difference}.\n"
            + $"----------------------------- Original: Around Line {original.Extent.StartLineNumber} -----------------------------\n"
            + Shorten(original.Extent.Text)
            + $"\n----------------------------- Formatted: Around Line {formatted.Extent.StartLineNumber} -----------------------------\n"
            + Shorten(formatted.Extent.Text);
    }

    private static string Shorten(string text)
    {
        var lines = text.Replace("\r\n", "\n").Split('\n');
        return lines.Length <= 10 ? text : string.Join("\n", lines.Take(10)) + "\n...";
    }
}
