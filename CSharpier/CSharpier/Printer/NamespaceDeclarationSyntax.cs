using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO 2 there are some weird edge cases in here that are in the comments test, they probably won't show up in the real world
        private Doc PrintNamespaceDeclarationSyntax(NamespaceDeclarationSyntax node)
        {
            this.printNewLinesInLeadingTrivia.Push(true);
            var parts = new Parts();
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintLeadingTrivia(node.NamespaceKeyword));
            parts.Add(node.NamespaceKeyword.Text);
            parts.Push(this.PrintTrailingTrivia(node.NamespaceKeyword));
            this.printNewLinesInLeadingTrivia.Pop();
            parts.Add(String(" "));
            parts.Add(this.Print(node.Name));
            var hasMembers = node.Members.Count > 0;
            var hasUsing = node.Usings.Count > 0;
            var hasExterns = node.Externs.Count > 0;
            parts.Push(this.PrintLeadingTrivia(node.OpenBraceToken));
            if (hasMembers || hasUsing || hasExterns) {
                parts.Push(HardLine, "{");
                parts.Push(this.PrintTrailingTrivia(node.OpenBraceToken));
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
                parts.Push(this.PrintLeadingTrivia(node.CloseBraceToken));
            } else
            {
                parts.Push(SpaceIfNoPreviousComment, "{");
                var trailingStart = this.PrintTrailingTrivia(node.OpenBraceToken);
                var leadingEnd = this.PrintLeadingTrivia(node.CloseBraceToken);
                if (trailingStart == null && leadingEnd == null)
                {
                    parts.Push(" ");
                }
                else
                {
                    parts.Push(trailingStart);
                    parts.Push(leadingEnd);
                }
            }
            parts.Push("}");
            parts.Push(this.PrintTrailingTrivia(node.CloseBraceToken));
            
            return Concat(parts);
        }
    }
}
