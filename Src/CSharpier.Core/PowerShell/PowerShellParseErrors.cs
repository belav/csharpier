using System.Management.Automation.Language;

namespace CSharpier.Core.PowerShell;

internal static class PowerShellParseErrors
{
    // These errors depend on which modules, DSC resources, and types are installed on the machine.
    // The parser still returns a complete tree for them.
    private static readonly HashSet<string> ResolutionErrorIds = new(StringComparer.Ordinal)
    {
        "ResourceNotDefined",
        "ModuleNotFoundDuringParse",
        "TypeNotFound",
        "CustomAttributeTypeNotFound",
        "RequiresModuleInvalid",
        "WorkflowNotSupportedInPowerShellCore",
        "DscResourceMissingSetMethod",
    };

    // On machines without DSC (Linux and macOS by default) the parser fails to load the DSC
    // keywords, reports the exception as a parse error, and drops the configuration statement
    // from the tree.
    internal static bool IsDscUnavailable(ParseError error)
    {
        return error.Message.Contains("DscClassCache", StringComparison.Ordinal);
    }

    internal static bool IsBlocking(ParseError error)
    {
        return !ResolutionErrorIds.Contains(error.ErrorId);
    }
}
