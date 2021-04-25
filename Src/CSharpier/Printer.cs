using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpier.SyntaxPrinter;

namespace CSharpier
{
    // TODO 1 can I use source generators for some stuff?
    // https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/
    public partial class Printer
    {
        public Printer()
        {
            SyntaxNodes.Initialize(this);
        }

        private Doc PrintAttributeLists(
            SyntaxNode node,
            SyntaxList<AttributeListSyntax> attributeLists
        ) {
            if (attributeLists.Count == 0)
            {
                return Doc.Null;
            }

            var docs = new List<Doc>();
            Doc separator = node is TypeParameterSyntax ||
                node is ParameterSyntax
                ? Docs.Line
                : Docs.HardLine;
            docs.Add(
                Docs.Join(
                    separator,
                    attributeLists.Select(this.PrintAttributeListSyntax)
                )
            );

            if (!(node is ParameterSyntax))
            {
                docs.Add(separator);
            }

            return Docs.Concat(docs);
        }

        private Doc PrintConstraintClauses(
            IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses
        ) {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return Doc.Null;
            }

            var docs = new List<Doc>
            {
                Docs.Indent(
                    Docs.HardLine,
                    Docs.Join(
                        Docs.HardLine,
                        constraintClausesList.Select(
                            this.PrintTypeParameterConstraintClauseSyntax
                        )
                    )
                )
            };

            return Docs.Concat(docs);
        }

        private Doc PrintBaseFieldDeclarationSyntax(
            BaseFieldDeclarationSyntax node
        ) {
            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers)
            };
            if (
                node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax
            ) {
                docs.Add(
                    SyntaxTokens.PrintWithSuffix(
                        eventFieldDeclarationSyntax.EventKeyword,
                        " "
                    )
                );
            }

            docs.Add(SyntaxNodes.Print(node.Declaration));
            docs.Add(SyntaxTokens.Print(node.SemicolonToken));
            return Docs.Concat(docs);
        }
    }
}
