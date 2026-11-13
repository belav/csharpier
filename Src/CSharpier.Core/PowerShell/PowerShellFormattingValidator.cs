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
    private const int MaxDepth = 500;

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
            }
        );
    }

    private string? Validate(CancellationToken cancellationToken)
    {
        var originalAst = Parser.ParseInput(
            originalCode,
            out var originalTokens,
            out var originalErrors
        );
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

        return this.CompareNodes(originalAst, formattedAst, 0, cancellationToken);
    }

    private static List<Token> GetComments(Token[] tokens)
    {
        return tokens.Where(o => o.Kind == TokenKind.Comment).ToList();
    }

    private static string NormalizeComment(string comment)
    {
        return string.Join("\n", comment.Replace("\r\n", "\n").Split('\n').Select(o => o.Trim()));
    }

    private string? CompareNodes(
        Ast original,
        Ast formatted,
        int depth,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (original.GetType() != formatted.GetType())
        {
            return Describe(
                original,
                formatted,
                $"the node type changed from {original.GetType().Name} to {formatted.GetType().Name}"
            );
        }

        if (depth >= MaxDepth)
        {
            return NormalizeText(original.Extent.Text) == NormalizeText(formatted.Extent.Text)
                ? null
                : Describe(original, formatted, "the text of a deeply nested node changed");
        }

        foreach (var property in GetProperties(original.GetType()))
        {
            object? originalValue;
            object? formattedValue;
            try
            {
                originalValue = property.GetValue(original);
                formattedValue = property.GetValue(formatted);
            }
            catch
            {
                continue;
            }

            var result = this.CompareValues(
                originalValue,
                formattedValue,
                original,
                formatted,
                property.Name,
                depth,
                cancellationToken
            );
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    private string? CompareValues(
        object? originalValue,
        object? formattedValue,
        Ast originalOwner,
        Ast formattedOwner,
        string propertyName,
        int depth,
        CancellationToken cancellationToken
    )
    {
        if (originalValue is null || formattedValue is null)
        {
            return originalValue is null && formattedValue is null
                ? null
                : Describe(originalOwner, formattedOwner, $"{propertyName} was added or removed");
        }

        switch (originalValue)
        {
            case Ast originalAst:
                return formattedValue is Ast formattedAst
                    ? this.CompareNodes(originalAst, formattedAst, depth + 1, cancellationToken)
                    : Describe(originalOwner, formattedOwner, $"{propertyName} changed");
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

                for (var index = 0; index < originalTuple.Length; index++)
                {
                    var result = this.CompareValues(
                        originalTuple[index],
                        formattedTuple[index],
                        originalOwner,
                        formattedOwner,
                        propertyName,
                        depth,
                        cancellationToken
                    );
                    if (result is not null)
                    {
                        return result;
                    }
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

                for (var index = 0; index < originalItems.Count; index++)
                {
                    var result = this.CompareValues(
                        originalItems[index],
                        formattedItems[index],
                        originalOwner,
                        formattedOwner,
                        propertyName,
                        depth,
                        cancellationToken
                    );
                    if (result is not null)
                    {
                        return result;
                    }
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
        return PropertiesByType.GetOrAdd(
            type,
            static type =>
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

    private static string NormalizeText(string value)
    {
        return string.Join(" ", value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
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
