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
            Node.Initialize(this);
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
                ? Doc.Line
                : Doc.HardLine;
            docs.Add(
                Doc.Join(
                    separator,
                    attributeLists.Select(this.PrintAttributeListSyntax)
                )
            );

            if (!(node is ParameterSyntax))
            {
                docs.Add(separator);
            }

            return Doc.Concat(docs);
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
                Doc.Indent(
                    Doc.HardLine,
                    Doc.Join(
                        Doc.HardLine,
                        constraintClausesList.Select(
                            this.PrintTypeParameterConstraintClauseSyntax
                        )
                    )
                )
            };

            return Doc.Concat(docs);
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
                    Token.PrintWithSuffix(
                        eventFieldDeclarationSyntax.EventKeyword,
                        " "
                    )
                );
            }

            docs.Add(Node.Print(node.Declaration));
            docs.Add(Token.Print(node.SemicolonToken));
            return Doc.Concat(docs);
        }
    }
}
