using System;
using System.Collections.Generic;
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
            
            Doc body = " ";
            if (statements.Count > 0)
            {
                body = Concat(Indent(Concat(separator, Join(actualEndOfLine, statements.Select(this.Print)))), separator);
            }

            var parts = new Parts(Line, "{", body, "}");
            // TODO printTrailingComments(node, parts, "closeBraceToken");
            return Group(Concat(parts));
        }

        private Doc PrintCommaList(IEnumerable<Doc> docs)
        {
            return Join(Concat(String(","), Line), docs);
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

        private Doc PrintBaseTypeDeclarationSyntax(BaseTypeDeclarationSyntax node)
        {
            TypeParameterListSyntax typeParameterList = null;
            var hasConstraintClauses = false;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            var hasMembers = false;
            string keyword = null;
            var memberSeparator = HardLine;
            var members = Enumerable.Empty<CSharpSyntaxNode>();

            if (node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                typeParameterList = typeDeclarationSyntax.TypeParameterList;
                constraintClauses = typeDeclarationSyntax.ConstraintClauses;
                hasConstraintClauses = typeDeclarationSyntax.ConstraintClauses.Count > 0;
                members = typeDeclarationSyntax.Members;
                hasMembers = typeDeclarationSyntax.Members.Count > 0;
                if (node is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    keyword = classDeclarationSyntax.Keyword.Text;
                }
                else if (node is StructDeclarationSyntax structDeclarationSyntax)
                {
                    keyword = structDeclarationSyntax.Keyword.Text;
                }
                else if (node is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
                {
                    keyword = interfaceDeclarationSyntax.Keyword.Text;
                }
            }
            else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
            {
                members = enumDeclarationSyntax.Members;
                hasMembers = enumDeclarationSyntax.Members.Count > 0;
                keyword = enumDeclarationSyntax.EnumKeyword.Text;
                memberSeparator = Concat(String(","), HardLine);
            }

            var parts = new Parts();

            this.PrintExtraNewLines(node, parts);

            this.PrintAttributeLists(node, node.AttributeLists, parts);
            // TODO printLeadingComments(node, parts, String("modifiers"), String("keyword"), String("identifier"));
            parts.Add(this.PrintModifiers(node.Modifiers));
            if (keyword != null)
            {
                parts.Add(keyword);
            }

            parts.Push(String(" "), node.Identifier.Text);
            if (typeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (node.BaseList != null)
            {
                parts.Add(this.PrintBaseListSyntax(node.BaseList));
            }

            this.PrintConstraintClauses(node, constraintClauses, parts);

            if (hasMembers)
            {
                parts.Add(Concat(hasConstraintClauses ? "" : HardLine, "{"));
                parts.Add(Indent(Concat(HardLine, Join(memberSeparator, members.Select(this.Print)))));
                parts.Add(HardLine);
                parts.Add(String("}"));
            }
            else
            {
                parts.Push(hasConstraintClauses ? "" : " ", "{", " ", "}");
            }

            return Concat(parts);
        }


        // TODO 0 how do I really do extra new lines?
        private void PrintExtraNewLines(BaseTypeDeclarationSyntax node, Parts parts)
        {
            // TODO attribute lists?
            if (node.Modifiers.Count > 0)
            {
                foreach (var trivia in node.Modifiers[0].LeadingTrivia)
                {
                    if (trivia.Kind() == SyntaxKind.EndOfLineTrivia)
                    {
                        parts.Push(HardLine);
                    }
                    else if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                    {
                        return;
                    }
                }
            }

            if (node is ClassDeclarationSyntax classDeclarationSyntax)
            {
                foreach (var trivia in classDeclarationSyntax.Keyword.LeadingTrivia)
                {
                    if (trivia.Kind() == SyntaxKind.EndOfLineTrivia)
                    {
                        parts.Push(HardLine);
                    }
                    else if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                    {
                        return;
                    }
                }
            }
        }

        private Doc PrintBaseMethodDeclarationSyntax(CSharpSyntaxNode node)
        {
            SyntaxList<AttributeListSyntax> attributeLists;
            SyntaxTokenList? modifiers = null;
            TypeSyntax returnType = null;
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null;
            TypeParameterListSyntax typeParameterList = null;
            string identifier = null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            ParameterListSyntax parameterList = null;
            BlockSyntax body = null;
            ArrowExpressionClauseSyntax expressionBody = null;
            if (node is BaseMethodDeclarationSyntax baseMethodDeclarationSyntax)
            {
                attributeLists = baseMethodDeclarationSyntax.AttributeLists;
                modifiers = baseMethodDeclarationSyntax.Modifiers;
                parameterList = baseMethodDeclarationSyntax.ParameterList;
                body = baseMethodDeclarationSyntax.Body;
                expressionBody = baseMethodDeclarationSyntax.ExpressionBody;
                if (node is MethodDeclarationSyntax methodDeclarationSyntax)
                {
                    returnType = methodDeclarationSyntax.ReturnType;
                    explicitInterfaceSpecifier = methodDeclarationSyntax.ExplicitInterfaceSpecifier;
                    identifier = methodDeclarationSyntax.Identifier.Text;
                    typeParameterList = methodDeclarationSyntax.TypeParameterList;
                    constraintClauses = methodDeclarationSyntax.ConstraintClauses;
                }
            }
            else if (node is LocalFunctionStatementSyntax localFunctionStatementSyntax)
            {
                attributeLists = localFunctionStatementSyntax.AttributeLists;
                modifiers = localFunctionStatementSyntax.Modifiers;
                returnType = localFunctionStatementSyntax.ReturnType;
                identifier = localFunctionStatementSyntax.Identifier.Text;
                typeParameterList = localFunctionStatementSyntax.TypeParameterList;
                parameterList = localFunctionStatementSyntax.ParameterList;
                constraintClauses = localFunctionStatementSyntax.ConstraintClauses;
                body = localFunctionStatementSyntax.Body;
                expressionBody = localFunctionStatementSyntax.ExpressionBody;
            }

            var parts = new Parts();
            //this.PrintExtraNewLines(node, String("attributeLists"), String("modifiers"), [String("returnType"), String("keyword")]);
            this.PrintAttributeLists(node, attributeLists, parts);
            // printLeadingComments(node, parts, String("modifiers"), String("returnType"), String("identifier"));
            if (modifiers.HasValue)
            {
                parts.Add(this.PrintModifiers(modifiers.Value));
            }

            if (returnType != null)
            {
                parts.Push(this.Print(returnType), String(" "));
            }

            if (explicitInterfaceSpecifier != null)
            {
                parts.Push(this.Print(explicitInterfaceSpecifier.Name), ".");
            }

            if (identifier != null)
            {
                parts.Add(identifier);
            }

            if (node is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax)
            {
                parts.Push(conversionOperatorDeclarationSyntax.ImplicitOrExplicitKeyword.Text,
                    " ",
                    conversionOperatorDeclarationSyntax.OperatorKeyword.Text,
                    " ",
                    this.Print(conversionOperatorDeclarationSyntax.Type));
            }
            else if (node is OperatorDeclarationSyntax operatorDeclarationSyntax)
            {
                parts.Push(this.Print(operatorDeclarationSyntax.ReturnType), " ", operatorDeclarationSyntax.OperatorKeyword.Text, " ", operatorDeclarationSyntax.OperatorToken.Text);
            }

            if (typeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (parameterList != null)
            {
                parts.Add(this.Print(parameterList));
            }

            this.PrintConstraintClauses(node, constraintClauses, parts);
            if (body != null)
            {
                parts.Add(this.PrintBlockSyntax(body));
            }
            else
            {
                if (expressionBody != null)
                {
                    parts.Add(this.PrintArrowExpressionClauseSyntax(expressionBody));
                }

                parts.Add(String(";"));
            }

            return Concat(parts);
        }
    }
}