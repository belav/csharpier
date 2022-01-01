namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

// TODO should abstract methods be treated like interface methods?
/*
    public abstract void WriteParameterSeparator();
    public abstract void WriteReturn();
*/

// TODO these are all the edge cases left + any failing tests
/*

// adds line before pragma
class ClassName
{
#pragma warning disable 0618
    public void DoStuff() { }   
#pragma warning restore 0618

    public void DoStuff() { }
}
// this should keep the line like so
class ClassName
{
    public void DoStuff() { }

#pragma warning disable 0618
    public void DoStuff() { }
#pragma warning restore 0618
}

// this loses a line before the first #if
public class DirectRouteFactoryContext
{
    private readonly string _actionName;

#if !ASPNETWEBAPI
    private readonly string _controllerName;
#endif

#if ASPNETWEBAPI
    private readonly string _prefix;
#else
    private readonly string _areaPrefix;
    private readonly string _controllerPrefix;
#endif
}

// this also loses the new line before the #if
using System.Text;
using System.Threading;
using System.Web.Http;

#if NETFX_CORE
using NameValueCollection = System.Net.Http.Formatting.HttpValueCollection;
#endif

namespace System.Net.Http.Formatting { }

// this loses a new line before the comment

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.Web.Helpers;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Microsoft.Web.Helpers")]
[assembly: AssemblyDescription("")]
[assembly: PreApplicationStartMethod(typeof(PreApplicationStartCode), "Start")]
[assembly: InternalsVisibleTo(
    "Microsoft.Web.Helpers.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9"
)]

// this loses the new line before pragma

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

#pragma warning disable 659 // overrides AddToHashCodeCombiner instead

namespace Microsoft.Web.Mvc.ExpressionUtil
{ }

 */

internal static class BaseTypeDeclaration
{
    public static Doc Print(BaseTypeDeclarationSyntax node)
    {
        ParameterListSyntax? parameterList = null;
        TypeParameterListSyntax? typeParameterList = null;
        var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
        var hasMembers = false;
        SyntaxToken? recordKeyword = null;
        SyntaxToken? keyword = null;
        Doc members = Doc.Null;
        SyntaxToken? semicolonToken = null;

        if (node is TypeDeclarationSyntax typeDeclarationSyntax)
        {
            typeParameterList = typeDeclarationSyntax.TypeParameterList;
            constraintClauses = typeDeclarationSyntax.ConstraintClauses;
            hasMembers = typeDeclarationSyntax.Members.Count > 0;
            if (typeDeclarationSyntax.Members.Count > 0)
            {
                members = Doc.Indent(
                    MembersWithForcedLines.Print(
                        typeDeclarationSyntax,
                        typeDeclarationSyntax.Members
                    )
                );
            }
            if (node is ClassDeclarationSyntax classDeclarationSyntax)
            {
                keyword = classDeclarationSyntax.Keyword;
            }
            else if (node is StructDeclarationSyntax structDeclarationSyntax)
            {
                keyword = structDeclarationSyntax.Keyword;
            }
            else if (node is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                keyword = interfaceDeclarationSyntax.Keyword;
            }
            else if (node is RecordDeclarationSyntax recordDeclarationSyntax)
            {
                recordKeyword = recordDeclarationSyntax.Keyword;
                keyword = recordDeclarationSyntax.ClassOrStructKeyword;
                parameterList = recordDeclarationSyntax.ParameterList;
            }

            semicolonToken = typeDeclarationSyntax.SemicolonToken;
        }
        else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
        {
            members = Doc.Indent(
                Doc.HardLine,
                SeparatedSyntaxList.Print(
                    enumDeclarationSyntax.Members,
                    EnumMemberDeclaration.Print,
                    Doc.HardLine
                )
            );
            hasMembers = enumDeclarationSyntax.Members.Count > 0;
            keyword = enumDeclarationSyntax.EnumKeyword;
            semicolonToken = enumDeclarationSyntax.SemicolonToken;
        }

        var docs = new List<Doc>();
        if (node.AttributeLists.Any())
        {
            docs.Add(AttributeLists.Print(node, node.AttributeLists));
        }

        if (node.Modifiers.Any())
        {
            docs.Add(Modifiers.Print(node.Modifiers));
        }

        if (recordKeyword != null)
        {
            docs.Add(Token.PrintWithSuffix(recordKeyword.Value, " "));
        }

        if (keyword != null)
        {
            docs.Add(Token.PrintWithSuffix(keyword.Value, " "));
        }

        docs.Add(Token.Print(node.Identifier));

        if (typeParameterList != null)
        {
            docs.Add(TypeParameterList.Print(typeParameterList));
        }

        if (parameterList != null)
        {
            docs.Add(ParameterList.Print(parameterList));
        }

        if (node.BaseList != null)
        {
            var baseListDoc = Doc.Concat(
                Token.Print(node.BaseList.ColonToken),
                " ",
                Doc.Align(2, SeparatedSyntaxList.Print(node.BaseList.Types, Node.Print, Doc.Line))
            );

            docs.Add(Doc.Group(Doc.Indent(Doc.Line, baseListDoc)));
        }

        docs.Add(ConstraintClauses.Print(constraintClauses));

        if (hasMembers)
        {
            DocUtilities.RemoveInitialDoubleHardLine(members);

            docs.Add(
                Doc.HardLine,
                Token.Print(node.OpenBraceToken),
                members,
                Doc.HardLine,
                Token.Print(node.CloseBraceToken)
            );
        }
        else if (node.OpenBraceToken.Kind() != SyntaxKind.None)
        {
            Doc separator = node.CloseBraceToken.LeadingTrivia.Any() ? Doc.Line : " ";

            docs.Add(
                separator,
                Token.Print(node.OpenBraceToken),
                separator,
                Token.Print(node.CloseBraceToken)
            );
        }

        if (semicolonToken.HasValue)
        {
            docs.Add(Token.Print(semicolonToken.Value));
        }

        return Doc.Concat(docs);
    }
}
