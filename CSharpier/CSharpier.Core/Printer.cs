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
            return Join(Concat(",", Line), docs);
        }

        private Doc PrintAttributeLists(SyntaxNode node, SyntaxList<AttributeListSyntax> attributeLists)
        {
            if (attributeLists.Count == 0)
            {
                return null;
            }

            this.printNewLinesInLeadingTrivia.TryPeek(out var printNewLines);
            if (printNewLines)
            {
                this.printNewLinesInLeadingTrivia.Pop();
                this.printNewLinesInLeadingTrivia.Push(false);
            }
            
            var parts = new Parts();
            var separator = node is TypeParameterSyntax || node is ParameterSyntax ? Line : HardLine;
            if (printNewLines)
            {
                foreach (var leadingTrivia in attributeLists[0].OpenBracketToken.LeadingTrivia)
                {
                    if (leadingTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
                    {
                        parts.Push(HardLine);
                    }
                    else
                    {
                        break;
                    }
                }   
            }
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

            return Concat(parts);
        }

        private Doc PrintModifiers(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
            {
                return "";
            }

            var parts = new Parts();
            foreach (var modifier in modifiers)
            {
                parts.Push(this.PrintLeadingTrivia(modifier.LeadingTrivia));
                parts.Push(modifier.Text);
                parts.Push(this.PrintTrailingTrivia(modifier));
                parts.Push(SpaceIfNoPreviousComment);
            }

            return Group(Concat(parts));
        }

        // TODO 0 I don't know that we really wanna call this, but we could maybe use this for printing new lines?
        // it looks like the methods GetLeadingTrivia calls could be used to find all the leading newlines
        private Doc PrintLeadingTrivia(CSharpSyntaxNode node)
        {
            if (!node.HasLeadingTrivia)
            {
                return null;
            }

            return this.PrintLeadingTrivia(node.GetLeadingTrivia());
        }

        private Doc PrintLeadingTrivia(SyntaxToken syntaxToken)
        {
            return this.PrintLeadingTrivia(syntaxToken.LeadingTrivia);
        }
        
        private readonly Stack<bool> printNewLinesInLeadingTrivia = new();

        private Doc PrintLeadingTrivia(SyntaxTriviaList leadingTrivia)
        {
            var parts = new Parts();
            
            this.printNewLinesInLeadingTrivia.TryPeek(out var doNewLines);

            var hadDirective = false;
            for (var x = 0; x < leadingTrivia.Count; x++)
            {
                var trivia = leadingTrivia[x];

                if (doNewLines && trivia.Kind() == SyntaxKind.EndOfLineTrivia)
                {
                    SyntaxKind? kind = null;
                    if (x < leadingTrivia.Count - 1)
                    {
                        kind = leadingTrivia[x + 1].Kind();
                    }
                    // TODO this probably needs multiline trivia too
                    // TODO this may screw up with regions that aren't at the beginning of the line?
                    if (!kind.HasValue || kind == SyntaxKind.SingleLineCommentTrivia || kind == SyntaxKind.EndOfLineTrivia || kind == SyntaxKind.WhitespaceTrivia)
                    {
                        parts.Push(HardLine);
                    }
                }
                if (trivia.Kind() != SyntaxKind.EndOfLineTrivia && trivia.Kind() != SyntaxKind.WhitespaceTrivia)
                {
                    if (doNewLines)
                    {
                        doNewLines = false;
                        this.printNewLinesInLeadingTrivia.Pop();
                        this.printNewLinesInLeadingTrivia.Push(false);
                    }
                }
                if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                {
                    parts.Push(LeadingComment(trivia.ToString(), CommentType.SingleLine));
                }
                else if (trivia.Kind() == SyntaxKind.DisabledTextTrivia)
                {
                    parts.Push(LiteralLine, trivia.ToString().TrimEnd('\n', '\r'));
                }
                else if (trivia.Kind() == SyntaxKind.IfDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.ElseDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.ElifDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.EndIfDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.LineDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.ErrorDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.WarningDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.PragmaWarningDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.PragmaChecksumDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.DefineDirectiveTrivia
                         || trivia.Kind() == SyntaxKind.UndefDirectiveTrivia)
                {
                    hadDirective = true;
                    parts.Push(LiteralLine, trivia.ToString());
                }
                else if (trivia.Kind() == SyntaxKind.RegionDirectiveTrivia 
                         || trivia.Kind() == SyntaxKind.EndRegionDirectiveTrivia)
                {
                    var triviaText = trivia.ToString();
                    if (x > 0 && leadingTrivia[x - 1].Kind() == SyntaxKind.WhitespaceTrivia)
                    {
                        triviaText = leadingTrivia[x - 1] + triviaText;
                    }
                    
                    hadDirective = true;
                    parts.Push(LiteralLine, triviaText);
                }
            }

            if (hadDirective)
            {
                parts.Push(HardLine);
            }

            return parts.Count > 0 ? Concat(parts) : null;
        }

        private Doc PrintTrailingTrivia(CSharpSyntaxNode node)
        {
            if (!node.HasTrailingTrivia)
            {
                return null;
            }

            return this.PrintTrailingTrivia(node.GetTrailingTrivia());
        }

        private Doc PrintTrailingTrivia(SyntaxToken node)
        {
            return this.PrintTrailingTrivia(node.TrailingTrivia);
        }
        
        private Doc PrintTrailingTrivia(SyntaxTriviaList trailingTrivia)
        {
            var parts = new Parts();
            foreach (var trivia in trailingTrivia)
            {
                if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                {
                    parts.Push(TrailingComment(trivia.ToString(), CommentType.SingleLine));
                }
                else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
                {
                    parts.Push(" ", trivia.ToString(), Line);
                }
            }
            
            return parts.Count > 0 ? Concat(parts) : null;
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
            this.printNewLinesInLeadingTrivia.Push(true);
            var parts = new Parts();
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
            {
                parts.Push(eventFieldDeclarationSyntax.EventKeyword.Text, " ");
            }

            parts.Push(this.Print(node.Declaration));
            parts.Push(";");
            parts.Push(this.PrintTrailingTrivia(node.SemicolonToken));
            return Concat(parts);
        }
    }
}