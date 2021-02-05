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
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(String("namespace"));
            parts.Add(String(" "));
            parts.Add(this.Print(node.Name));
            var hasMembers = node.Members.Count > 0;
            var hasUsing = node.Usings.Count > 0;
            var hasExterns = node.Externs.Count > 0;
            if (hasMembers || hasUsing || hasExterns) {
                parts.Push(HardLine, String("{"));
                var innerParts = new Parts();
                innerParts.Add(HardLine);
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
                innerParts.RemoveAt(innerParts.Count - 1);
                parts.Add(Indent(Concat(innerParts)));
                parts.Add(HardLine);
                parts.Add(String("}"));
            } else {
                parts.Push(String(" "), String("{"), String(" "), String("}"));
            }
            return Concat(Concat(parts));
        }
    }
}
