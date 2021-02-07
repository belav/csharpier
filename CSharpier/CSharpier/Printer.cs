using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        public static Doc BreakParent = new BreakParent();

        public static Doc HardLine = Concat(new LineDoc { Type = LineDoc.LineType.Hard }, BreakParent);
        public static Doc Line = new LineDoc { Type = LineDoc.LineType.Normal };
        public static Doc SoftLine = new LineDoc { Type = LineDoc.LineType.Soft };

        public static Doc Concat(Parts parts)
        {
            return new Concat
            {
                Parts = parts
            };
        }

        public static Doc Concat(params Doc[] parts)
        {
            return new Concat
            {
                Parts = new List<Doc>(parts)
            };
        }

        public static Doc String(string value)
        {
            return new StringDoc(value);
        }

        public static Doc Join(Doc separator, IEnumerable<Doc> array)
        {
            var parts = new Parts();

            var list = array.ToList();

            if (list.Count == 1)
            {
                return list[0];
            }

            for (var x = 0; x < list.Count; x++)
            {
                if (x != 0)
                {
                    parts.Add(separator);
                }

                parts.Add(list[x]);
            }

            return Concat(parts);
        }

        public static Doc Group(Doc contents)
        {
            return new Group
            {
                Contents = contents,
                // TODO group options if I use them
                // id: opts.id,
                // break: !!opts.shouldBreak,
                // expandedStates: opts.expandedStates,
            };
        }

        public static Doc Indent(Doc contents)
        {
            return new IndentDoc
            {
                Contents = contents
            };
        }

        private Doc PrintCommaList(IEnumerable<Doc> docs)
        {
            return Join(Concat(String(","), Line), docs);
        }
        
        private void PrintAttributeLists(SyntaxNode node, SyntaxList<AttributeListSyntax> attributeLists, List<Doc> parts)
        {
            if (attributeLists.Count == 0)
            {
                return;
            }

            var separator = node is TypeParameterSyntax || node is ParameterSyntax ? Line : HardLine;
            parts.Add(
                Join(
                    separator,
                    attributeLists.Select(this.PrintAttributeListSyntax)
                )
            );

            if (!(node is ParameterSyntax))
            {
                parts.Add(separator);
            }
        }

        private Doc PrintModifiers(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
            {
                return "";
            }

            return Concat(Join(" ", modifiers.Select(o => String(o.Text))), " ");

            // var parts = new Parts();
            // foreach (var modifier in modifiers)
            // {
            //     foreach (var leadingTrivia in modifier.LeadingTrivia)
            //     {
            //         if (leadingTrivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
            //         {
            //             parts.Push(leadingTrivia.ToString(), Environment.NewLine);
            //         }
            //     }
            //     parts.Push(modifier.Text);
            //
            //     var hasTrailing = false;
            //     foreach (var trailingTrivia in modifier.TrailingTrivia)
            //     {
            //         if (trailingTrivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
            //         {
            //             parts.Push(" ", trailingTrivia.ToString());
            //             hasTrailing = true;
            //         }
            //     }
            //
            //     if (!hasTrailing)
            //     {
            //         parts.Push(Line);
            //     }
            // }
            //
            // return Concat(parts);
        }

        private Doc PrintStatements<T>(IReadOnlyList<T> statements, Doc separator, Doc endOfLineDoc = null)
            where T : SyntaxNode
        {
            var actualEndOfLine = endOfLineDoc != null ? Concat(endOfLineDoc, separator) : separator;
            
            Doc body = " ";
            if (statements.Count > 0)
            {
                body = Concat(Indent(Concat(separator, Join(actualEndOfLine, statements.Select(this.Print)))), separator);
            }

            var parts = new Parts(Line, "{", body, "}");
            // TODO printTrailingComments(node, parts, "closeBraceToken");
            return Group(Concat(parts));
        }

        private void PrintConstraintClauses(SyntaxNode node, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses, List<Doc> parts)
        {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return;
            }


            parts.Add(
                Indent(
                    Concat(
                        HardLine,
                        Join(
                            HardLine,
                            constraintClausesList.Select(this.PrintTypeParameterConstraintClauseSyntax)
                        )
                    )
                )
            );

            if (
                !(node is DelegateDeclarationSyntax)
                && !(node is MethodDeclarationSyntax)
                && !(node is LocalFunctionStatementSyntax)
            )
            {
                parts.Add(HardLine);
            }
        }

        private Doc PrintLeftRightOperator(SyntaxNode node, SyntaxNode left, SyntaxToken operatorToken, SyntaxNode right)
        {
            var parts = new Parts();
            // TODO printExtraNewLines(node, parts, ["left", "identifier"]);
            // TODO printLeadingComments(node, parts, ["left", "identifier"]);
            parts.Push(
                this.Print(left),
                " ",
                operatorToken.Text,
                " ",
                this.Print(right)
            );
            return Concat(parts);
        }

        private Doc PrintBaseFieldDeclarationSyntax(BaseFieldDeclarationSyntax node)
        {
            var parts = new Parts();
            // printExtraNewLines(node, parts, "attributeLists", "modifiers", "eventKeyword", "declaration");
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            // printLeadingComments(node, parts, "modifiers", "eventKeyword", "declaration");
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
            {
                parts.Push(eventFieldDeclarationSyntax.EventKeyword.Text, " ");
            }

            parts.Push(this.Print(node.Declaration));
            parts.Push(";");
            // printTrailingComments(node, parts, "semicolonToken");
            return Concat(parts);
        }
    }
}