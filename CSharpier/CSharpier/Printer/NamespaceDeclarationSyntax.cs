using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNamespaceDeclarationSyntax(NamespaceDeclarationSyntax node)
        {
            var parts = new Parts();
            //this.PrintExtraNewLines(node, String("attributeLists"), String("modifiers"), String("namespaceKeyword"));
            this.PrintAttributeLists(node.AttributeLists, parts);
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(String("namespace"));
            parts.Push(String(" "));
            parts.Push(this.Print(node.Name));
            var hasMembers = node.Members.Count > 0;
            var hasUsing = node.Usings.Count > 0;
            var hasExterns = node.Externs.Count > 0;
            if (hasMembers || hasUsing || hasExterns) {
                parts.Push(HardLine, String("{"));
                var innerParts = new Parts();
                innerParts.Push(HardLine);
                if (hasExterns) {
                    innerParts.Push(
                        Join(
                            HardLine,
                            node.Externs.Select(this.PrintExternAliasDirectiveSyntax)
                        ),
                        HardLine
                    );
                }
                if (hasUsing) {
                    innerParts.Push(
                        Join(
                            HardLine,
                            node.Usings.Select(this.PrintUsingDirectiveSyntax)
                        ),
                        HardLine
                    );
                }
                if (hasMembers) {
                    innerParts.Push(Join(HardLine, node.Members.Select(this.Print)), HardLine);
                }
                innerParts.TheParts.RemoveAt(innerParts.TheParts.Count - 1);
                parts.Push(Indent(Concat(innerParts)));
                parts.Push(HardLine);
                parts.Push(String("}"));
            } else {
                parts.Push(String(" "), String("{"), String(" "), String("}"));
            }
            return Concat(Concat(parts));
        }
    }
}
