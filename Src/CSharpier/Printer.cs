using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpier.SyntaxPrinter;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;

namespace CSharpier
{
    // TODO 1 can I use source generators for some stuff?
    // https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/
    public partial class Printer
    {
        // TODO Partial
        public Doc PrintAttributeLists(
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
                Doc.Join(separator, attributeLists.Select(AttributeList.Print))
            );

            if (!(node is ParameterSyntax))
            {
                docs.Add(separator);
            }

            return Doc.Concat(docs);
        }

        // TODO Partial
        public Doc PrintConstraintClauses(
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
                            TypeParameterConstraintClause.Print
                        )
                    )
                )
            };

            return Doc.Concat(docs);
        }

        public Doc PrintBaseFieldDeclarationSyntax(
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
