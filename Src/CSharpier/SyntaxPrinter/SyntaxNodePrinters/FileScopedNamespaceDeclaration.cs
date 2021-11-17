using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FileScopedNamespaceDeclaration
{
    public static Doc Print(FileScopedNamespaceDeclarationSyntax node)
    {
        var docs = new List<Doc>()
        {
            ExtraNewLines.Print(node),
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers),
            Token.Print(node.NamespaceKeyword),
            " ",
            Node.Print(node.Name),
            Token.Print(node.SemicolonToken)
        };

        docs.Add(Doc.HardLine);

        if (node.Externs.Any())
        {
            docs.Add(Doc.Join(Doc.HardLine, node.Externs.Select(Node.Print)), Doc.HardLine);
        }

        if (node.Usings.Any())
        {
            docs.Add(Doc.Join(Doc.HardLine, node.Usings.Select(Node.Print)), Doc.HardLine);
        }

        if (node.Members.Any())
        {
            docs.Add(Doc.Join(Doc.HardLine, node.Members.Select(Node.Print)), Doc.HardLine);
        }

        return Doc.Concat(docs);
    }
}
