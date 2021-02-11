using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        public static readonly Doc BreakParent = new BreakParent();

        // TODO kill?
        public static readonly Doc SpaceIfNoPreviousComment = new SpaceIfNoPreviousComment();
        public static readonly Doc HardLine = Concat(new LineDoc { Type = LineDoc.LineType.Hard }, BreakParent);
        public static readonly Doc LiteralLine = Concat(new LineDoc { Type = LineDoc.LineType.Hard, IsLiteral = true }, BreakParent);
        public static readonly Doc Line = new LineDoc { Type = LineDoc.LineType.Normal };
        public static readonly Doc SoftLine = new LineDoc { Type = LineDoc.LineType.Soft };

        public static Doc LeadingComment(string comment, CommentType commentType)
        {
            return new LeadingComment
            {
                Type = commentType,
                Comment = comment,
            };
        }

        public static Doc TrailingComment(string comment, CommentType commentType)
        {
            return new TrailingComment
            {
                Type = commentType,
                Comment = comment,
            };
        }


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
                    parts.Push(separator);
                }

                parts.Push(list[x]);
            }

            return Concat(parts);
        }

        public static Doc Group(params Doc[] contents)
        {
            return new Group
            {
                Contents = contents.Length == 0 ? contents[0] : Concat(contents),
                // TODO group options if I use them
                // id: opts.id,
                // break: !!opts.shouldBreak,
                // expandedStates: opts.expandedStates,
            };
        }

        public static Doc Indent(params Doc[] contents)
        {
            return new IndentDoc
            {
                Contents = contents.Length == 0 ? contents[0] : Concat(contents)
            };
        }

        private Doc PrintSeparatedSyntaxList<T>(SeparatedSyntaxList<T> list, Func<T, Doc> printFunc, Doc afterSeparator)
            where T : SyntaxNode
        {
            var parts = new Parts();
            for (var x = 0; x < list.Count; x++)
            {
                parts.Push(printFunc(list[x]));
                if (x < list.Count - 1)
                {
                    parts.Push(this.PrintSyntaxToken(list.GetSeparator(x), afterSeparator));
                }
            }

            return parts.Count == 0 ? null : Concat(parts);
        }
        
        private Doc PrintCommaList(IEnumerable<Doc> docs)
        {
            return Join(Concat(",", Line), docs);
        }

        private Doc PrintAttributeLists(SyntaxNode node, SyntaxList<AttributeListSyntax> attributeLists)
        {
            if (attributeLists.Count == 0)
            {
                return null;
            }

            var parts = new Parts();
            var separator = node is TypeParameterSyntax || node is ParameterSyntax ? Line : HardLine;
            parts.Push(
                Join(
                    separator,
                    attributeLists.Select(this.PrintAttributeListSyntax)
                )
            );

            if (!(node is ParameterSyntax))
            {
                parts.Push(separator);
            }

            return Concat(parts);
        }

        private Doc PrintModifiers(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
            {
                return null;
            }

            var parts = new Parts();
            foreach (var modifier in modifiers)
            {
                parts.Push(this.PrintSyntaxToken(modifier, " "));
            }

            return Group(Concat(parts));
        }

        // TODO 0 trivia!
        private Doc PrintStatements<T>(SyntaxToken openBraceToken, IReadOnlyList<T> statements, SyntaxToken closeBraceToken, Doc separator, Doc endOfLineDoc = null)
            where T : SyntaxNode
        {
            var actualEndOfLine = endOfLineDoc != null ? Concat(endOfLineDoc, separator) : separator;

            var parts = new Parts(Line, "{");
            if (statements.Count > 0)
            {
                parts.Push(Concat(Indent(separator, Join(actualEndOfLine, statements.Select(this.Print))), separator));
            }

            var leadingTrivia = this.PrintLeadingTrivia(closeBraceToken.LeadingTrivia);
            if (leadingTrivia != null)
            {
                parts.Push(leadingTrivia);
            }
            else if (statements.Count == 0)
            {
                parts.Push(" ");
            }

            parts.Push("}");
            parts.Push(this.PrintTrailingTrivia(closeBraceToken.TrailingTrivia));
            return Group(Concat(parts));
        }

        // TODO 0 change all methods to return docs
        private void PrintConstraintClauses(SyntaxNode node, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses, List<Doc> parts)
        {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return;
            }


            parts.Add(
                Indent(
                    HardLine,
                    Join(
                        HardLine,
                        constraintClausesList.Select(this.PrintTypeParameterConstraintClauseSyntax)
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
            parts.Push(
                this.Print(left),
                " ",
                this.PrintSyntaxToken(operatorToken),
                " ",
                this.Print(right)
            );
            return Concat(parts);
        }

        private Doc PrintBaseFieldDeclarationSyntax(BaseFieldDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
            {
                parts.Push(this.PrintSyntaxToken(eventFieldDeclarationSyntax.EventKeyword, " "));
            }

            parts.Push(this.Print(node.Declaration));
            parts.Push(this.PrintSyntaxToken(node.SemicolonToken));
            return Concat(parts);
        }
    }
}