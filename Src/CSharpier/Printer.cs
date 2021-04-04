using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    // TODO 1 can I use source generators for some stuff?
    // https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/
    public partial class Printer
    {
        private static Doc BreakParent => new BreakParent();

        // TODO 0 maybe all spaces should be this instead?
        public static Doc SpaceIfNoPreviousComment =>
            new SpaceIfNoPreviousComment();
        public static Doc HardLine =>
            Concat(new LineDoc { Type = LineDoc.LineType.Hard }, BreakParent);
        public static Doc LiteralLine =>
            Concat(
                new LineDoc { Type = LineDoc.LineType.Hard, IsLiteral = true },
                BreakParent
            );
        public static Doc Line =>
            new LineDoc { Type = LineDoc.LineType.Normal };
        public static Doc SoftLine =>
            new LineDoc { Type = LineDoc.LineType.Soft };

        public static Doc LeadingComment(
            string comment,
            CommentType commentType)
        {
            return new LeadingComment { Type = commentType, Comment = comment };
        }

        public static Doc TrailingComment(
            string comment,
            CommentType commentType)
        {
            return new TrailingComment
            {
                Type = commentType,
                Comment = comment,

            };
        }

        public static Doc Concat(Parts parts)
        {
            return new Concat(CleanParts(parts));
        }

        public static Doc Concat(params Doc[] parts)
        {
            return new Concat(CleanParts(parts));
        }

        public static List<Doc> CleanParts(IEnumerable<Doc> parts)
        {
            return parts.ToList();
#if DEBUG
            var newParts = new List<Doc>();
            foreach (var doc in parts)
            {
                if (doc is Concat concat)
                {
                    newParts.AddRange(CleanParts(concat.Parts));
                }
                else if (doc != null)
                {
                    newParts.Add(doc);
                }
            }
            return newParts;
#endif
            return parts.ToList();
        }

        public static Doc ForceFlat(params Doc[] contents)
        {
            return new ForceFlat
            {
                Contents = contents.Length == 0
                    ? contents[0]
                    : Concat(contents),

            };
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

        public static Doc Group(Parts parts)
        {
            return new Group
            {
                Contents = parts.Count == 1 ? parts[0] : Concat(parts),

            };
        }

        public static Doc Group(params Doc[] contents)
        {
            return new Group
            {
                Contents = contents.Length == 1
                    ? contents[0]
                    : Concat(contents),

            };
        }

        public static Doc Indent(params Doc[] contents)
        {
            return new IndentDoc
            {
                Contents = contents.Length == 1 ? contents[0] : Concat(contents)
            };
        }

        private Doc PrintSeparatedSyntaxList<T>(
            SeparatedSyntaxList<T> list,
            Func<T, Doc> printFunc,
            Doc afterSeparator)
            where T : SyntaxNode
        {
            var parts = new Parts();
            for (var x = 0; x < list.Count; x++)
            {
                parts.Push(printFunc(list[x]));

                if (x >= list.SeparatorCount)
                {
                    continue;
                }

                var isTrailingSeparator = x == list.Count - 1;

                parts.Push(
                    this.PrintSyntaxToken(
                        list.GetSeparator(x),
                        !isTrailingSeparator ? afterSeparator : null
                    )
                );
            }

            return parts.Count == 0 ? Doc.Null : Concat(parts);
        }

        private Doc PrintAttributeLists(
            SyntaxNode node,
            SyntaxList<AttributeListSyntax> attributeLists)
        {
            if (attributeLists.Count == 0)
            {
                return Doc.Null;
            }

            var parts = new Parts();
            var separator = node is TypeParameterSyntax
                || node is ParameterSyntax
                ? Line
                : HardLine;
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
                return Doc.Null;
            }

            var parts = new Parts();
            foreach (var modifier in modifiers)
            {
                parts.Push(
                    this.PrintSyntaxToken(modifier, afterTokenIfNoTrailing: " ")
                );
            }

            return Group(Concat(parts));
        }

        private Doc PrintConstraintClauses(
            SyntaxNode node,
            IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return Doc.Null;
            }


            var parts = new Parts(
                Indent(
                    HardLine,
                    Join(
                        HardLine,
                        constraintClausesList.Select(
                            this.PrintTypeParameterConstraintClauseSyntax
                        )
                    )
                )
            );

            return Concat(parts);
        }

        private Doc PrintBaseFieldDeclarationSyntax(
            BaseFieldDeclarationSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
            {
                parts.Push(
                    this.PrintSyntaxToken(
                        eventFieldDeclarationSyntax.EventKeyword,
                        " "
                    )
                );
            }

            parts.Push(this.Print(node.Declaration));
            parts.Push(this.PrintSyntaxToken(node.SemicolonToken));
            return Concat(parts);
        }
    }
}
