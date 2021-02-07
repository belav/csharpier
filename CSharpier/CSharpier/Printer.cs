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
            var boolean = true;
            return this.PrintModifiers(modifiers, ref boolean);
        }

        private Doc PrintModifiers(SyntaxTokenList modifiers, ref bool printedExtraNewLines)
        {
            if (modifiers.Count == 0)
            {
                return "";
            }

            var parts = new Parts();
            var isFirst = true;
            foreach (var modifier in modifiers)
            {
                this.PrintLeadingTrivia(modifier.LeadingTrivia, parts, ref printedExtraNewLines, !isFirst);
                parts.Push(modifier.Text);
                if (!this.PrintTrailingTrivia(modifier.TrailingTrivia, parts))
                {
                    parts.Push(" ");
                }

                isFirst = false;
            }
            
            return Group(Concat(parts));
        }

        private bool PrintLeadingTrivia(CSharpSyntaxNode node, Parts parts, ref bool printedExtraNewLines)
        {
            if (!node.HasLeadingTrivia)
            {
                return false;
            }
            
            return this.PrintLeadingTrivia(node.GetLeadingTrivia(), parts, ref printedExtraNewLines);
        }

        private bool PrintLeadingTrivia(SyntaxTriviaList leadingTrivia, Parts parts, ref bool printedExtraNewLines, bool includeHardLine = false)
        {
            var startCount = parts.Count;
            for (var x = 0; x < leadingTrivia.Count; x++)
            {
                var trivia = leadingTrivia[x];
                
                if (!printedExtraNewLines 
                    && trivia.Kind() == SyntaxKind.EndOfLineTrivia
                    && (x == leadingTrivia.Count - 1 || leadingTrivia[x + 1].Kind() != SyntaxKind.SingleLineCommentTrivia))
                {
                    parts.Push(HardLine);
                }
                    
                if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                {
                    printedExtraNewLines = true;
                    parts.Push(LeadingComment(trivia.ToString(), CommentType.SingleLine));
                }
                else if (trivia.Kind() == SyntaxKind.IfDirectiveTrivia)
                {
                    parts.Push(Environment.NewLine, trivia.ToString(), Environment.NewLine);
                }
                else if (trivia.Kind() == SyntaxKind.DisabledTextTrivia)
                {
                    parts.Push(trivia.ToString().TrimEnd('\n', '\r'));
                }
                else if (trivia.Kind() == SyntaxKind.ElseDirectiveTrivia)
                {
                    parts.Push(Environment.NewLine, trivia.ToString(), HardLine);
                }
                else if (trivia.Kind() == SyntaxKind.EndIfDirectiveTrivia)
                {
                    parts.Push(Environment.NewLine, trivia.ToString(), HardLine);
                }
            }

            return startCount != parts.Count;
        }
        
        private bool PrintTrailingTrivia(CSharpSyntaxNode node, Parts parts)
        {
            if (!node.HasTrailingTrivia)
            {
                return false;
            }
            
            return this.PrintTrailingTrivia(node.GetTrailingTrivia(), parts);
        }

        private bool PrintTrailingTrivia(SyntaxTriviaList trailingTrivia, Parts parts)
        {
            var hasTrivia = false;
            foreach (var trivia in trailingTrivia)
            {
                if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                {
                    parts.Push(TrailingComment(trivia.ToString(), CommentType.SingleLine));
                    hasTrivia = true;
                }
                else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
                {
                    parts.Push(" ", trivia.ToString(), Line);
                    hasTrivia = true;
                }
            }

            return hasTrivia;
        }
        
        private Doc PrintStatements<T>(SyntaxToken openBraceToken, IReadOnlyList<T> statements, SyntaxToken closeBraceToken, Doc separator, Doc endOfLineDoc = null)
            where T : SyntaxNode
        {
            var actualEndOfLine = endOfLineDoc != null ? Concat(endOfLineDoc, separator) : separator;

            var parts = new Parts(Line, "{");
            if (statements.Count > 0)
            {
                parts.Push(Concat(Indent(Concat(separator, Join(actualEndOfLine, statements.Select(this.Print)))), separator));
            }
            var ignore = false;
            var printedLeadingTrivia = this.PrintLeadingTrivia(closeBraceToken.LeadingTrivia, parts, ref ignore);
            if (!printedLeadingTrivia && statements.Count == 0)
            {
                parts.Push(" ");
            }
            parts.Push("}");
            this.PrintTrailingTrivia(closeBraceToken.TrailingTrivia, parts);
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