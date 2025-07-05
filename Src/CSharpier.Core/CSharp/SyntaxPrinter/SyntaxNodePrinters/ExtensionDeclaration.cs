using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

// TODO csharp-14 probably move this to a special case and use the BaseTypeDeclaration
// TODO csharp-14 add tests for this
internal static class ExtensionDeclaration
{
    public static Doc Print(ExtensionDeclarationSyntax node, PrintingContext context)
    {
        return node.ToFullString();
    }
}

// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#null-conditional-assignment
// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#unbound-generic-types-and-nameof
// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#implicit-span-conversions
// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#simple-lambda-parameters-with-modifiers
// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#the-field-keyword
// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#more-partial-members
// TODO csharp-14 https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14#user-defined-compound-assignment
