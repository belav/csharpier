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
        public static Doc SpaceIfNoPreviousComment =>
            Docs.SpaceIfNoPreviousComment;

        public static Doc HardLine => Docs.HardLine;

        public static Doc LiteralLine => Docs.LiteralLine;

        public static Doc Line => Docs.Line;

        public static Doc SoftLine => Docs.SoftLine;

        public static Doc LeadingComment(
            string comment,
            CommentType commentType) =>
            Docs.LeadingComment(comment, commentType);

        public static Doc TrailingComment(
            string comment,
            CommentType commentType) =>
            Docs.TrailingComment(comment, commentType);

        public static Doc Concat(Parts parts) => Docs.Concat(parts.ToArray());

        public static Doc Concat(params Doc[] parts) => Docs.Concat(parts);

        public static Doc ForceFlat(params Doc[] contents) =>
            Docs.ForceFlat(contents);

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

        public static Doc Group(Parts parts) => Docs.Group(parts.ToArray());

        public static Doc Group(List<Doc> contents) => Docs.Group(contents);

        public static Doc Group(params Doc[] contents) => Docs.Group(contents);

        public static Doc Indent(Parts parts) => Docs.Indent(parts.ToArray());

        public static Doc Indent(params Doc[] contents) =>
            Docs.Indent(contents);

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
                // TODO 1 this keeps trailing commas, that should probably be an option, for let's keep what appears to make finding "bad" code formats easier
                if (x < list.SeparatorCount)
                {
                    parts.Push(
                        this.PrintSyntaxToken(
                            list.GetSeparator(x),
                            afterSeparator
                        )
                    );
                }
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
