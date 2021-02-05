using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
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
                Contents = parts
            };
        }

        public static Doc Concat(params Doc[] parts)
        {
            return new Concat
            {
                Contents = new List<Doc>(parts)
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

            // TODO 0 this is good, but changes things from prettier
            // if (list.Count == 1)
            // {
            //     return list[0];
            // }
            
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
                // TODO options if I use them?
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

        private bool NotNullToken(SyntaxToken value)
        {
            return value.RawKind != 0;
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

            if (node is ParameterSyntax)
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

            return Concat(
                Join(
                    " ",
                    modifiers.Select(o => String(o.Text))
                ),
                " "
            );
        }

        private Doc PrintStatements<T>(IReadOnlyList<T> statements, Doc separator, Doc endOfLineDoc = null)
            where T : SyntaxNode
        {
            var actualEndOfLine = endOfLineDoc != null ? Concat(endOfLineDoc, separator) : separator;

            Doc beforeBody = Line;
            Doc body = " ";
            if (statements.Count > 0)
            {
                beforeBody = HardLine;
                body = Concat(Indent(Concat(separator, Join(actualEndOfLine, statements.Select(this.Print)))), separator);
            }

            // TODO 000000 for some reason, this Line doesn't print the same between csharpier and prettier in the MethodWithStatements test
            // it is almost like the prettier plugin is ignoring the width I send, but when I logged it directly in prettier source it had 80
            // but this line definitely seems to be wrong
            var parts = new Parts(beforeBody, "{", body, "}");
            // TODO printTrailingComments(node, parts, "closeBraceToken");
            return Group(Concat(parts));
        }

        private Doc PrintCommaList(IEnumerable<Doc> docs)
        {
            return Join(Concat(String(","), Line), docs);
        }

        private void PrintConstraintClauses(SyntaxNode node, SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses, List<Doc> parts)
        {
            if (constraintClauses.Count == 0)
            {
                return;
            }


            parts.Add(
                Indent(
                    Concat(
                        HardLine,
                        Join(
                            HardLine,
                            constraintClauses.Select(this.PrintTypeParameterConstraintClauseSyntax)
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
    }
}