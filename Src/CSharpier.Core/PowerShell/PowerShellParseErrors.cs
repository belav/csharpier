using System.Management.Automation.Language;

namespace CSharpier.Core.PowerShell;

internal static class PowerShellParseErrors
{
    // These errors depend on which modules, assemblies, DSC resources, and types are installed on
    // the machine. The parser still returns a complete tree for them.
    private static readonly HashSet<string> ResolutionErrorIds = new(StringComparer.Ordinal)
    {
        "ResourceNotDefined",
        "ModuleNotFoundDuringParse",
        "TypeNotFound",
        "CustomAttributeTypeNotFound",
        "RequiresModuleInvalid",
        "WorkflowNotSupportedInPowerShellCore",
        "DscResourceMissingSetMethod",
        "ErrorLoadingAssembly",
    };

    // On machines without DSC (Linux and macOS by default) loading the DSC keywords throws. The
    // parser catches the exception, reports it with the generic ParserError id on the name after
    // the configuration keyword, and drops the configuration statement from the tree. The message
    // is the exception text with its stack trace, which names internal types, so it is not used.
    internal static bool IsDscUnavailable(ParseError error, Token[] tokens)
    {
        if (error.ErrorId != "ParserError")
        {
            return false;
        }

        var index = Array.FindIndex(tokens, token =>
            token.Extent.StartOffset == error.Extent.StartOffset
        );
        for (index--; index >= 0; index--)
        {
            if (
                tokens[index].Kind
                is not (TokenKind.NewLine or TokenKind.LineContinuation or TokenKind.Comment)
            )
            {
                return tokens[index].Kind == TokenKind.Configuration;
            }
        }

        return false;
    }

    internal static bool IsBlocking(ParseError error)
    {
        return !ResolutionErrorIds.Contains(error.ErrorId);
    }
}
