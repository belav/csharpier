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

    internal static bool IsBlocking(ParseError error)
    {
        return !ResolutionErrorIds.Contains(error.ErrorId);
    }
}
