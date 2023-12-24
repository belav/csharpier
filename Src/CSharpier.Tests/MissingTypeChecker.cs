using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class MissingTypeChecker
{
    [Test]
    [Ignore(
        "this can be run to see what types are in a new language version that we don't handle yet"
    )]
    public void Ensure_There_Are_No_Missing_Types()
    {
        var directory = DirectoryFinder.FindParent("Src");

        var files = Directory
            .GetFiles(
                Path.Combine(directory.FullName, "CSharpier/SyntaxPrinter/SyntaxNodePrinters")
            )
            .Select(o => Path.GetFileNameWithoutExtension(o) + "Syntax")
            .ToList();

        var syntaxNodeTypes = typeof(CompilationUnitSyntax)
            .Assembly
            .GetTypes()
            .Where(o => !o.IsAbstract && typeof(CSharpSyntaxNode).IsAssignableFrom(o))
            .ToList();

        var missingTypes = new List<Type>();

        foreach (var nodeType in syntaxNodeTypes)
        {
            var type = nodeType;
            if (
                typeof(StructuredTriviaSyntax).IsAssignableFrom(type)
                || typeof(XmlNodeSyntax).IsAssignableFrom(type)
            )
            {
                continue;
            }

            if (this.ignored.Contains(nodeType.Name))
            {
                continue;
            }

            while (type != null)
            {
                if (files.Contains(type.Name))
                {
                    break;
                }

                type = type.BaseType;
            }

            if (type == null)
            {
                missingTypes.Add(nodeType);
            }
        }

        missingTypes.Should().BeEmpty();
    }

    private readonly HashSet<string> ignored =
        new()
        {
            "AccessorDeclarationSyntax",
            "AccessorListSyntax",
            "AttributeArgumentListSyntax",
            "AttributeArgumentSyntax",
            "AttributeSyntax",
            "AttributeTargetSpecifierSyntax",
            "BaseListSyntax",
            "CatchDeclarationSyntax",
            "CatchFilterClauseSyntax",
            "ConstructorInitializerSyntax",
            "ConversionOperatorMemberCrefSyntax",
            "CrefBracketedParameterListSyntax",
            "CrefParameterListSyntax",
            "CrefParameterSyntax",
            "ExplicitInterfaceSpecifierSyntax",
            "FunctionPointerCallingConventionSyntax",
            "FunctionPointerParameterListSyntax",
            "FunctionPointerParameterSyntax",
            "FunctionPointerUnmanagedCallingConventionListSyntax",
            "FunctionPointerUnmanagedCallingConventionSyntax",
            "IndexerMemberCrefSyntax",
            "InterpolationAlignmentClauseSyntax",
            "InterpolationFormatClauseSyntax",
            "JoinIntoClauseSyntax",
            "LineDirectivePositionSyntax",
            "NameMemberCrefSyntax",
            "OperatorMemberCrefSyntax",
            "OrderingSyntax",
            "PositionalPatternClauseSyntax",
            "PropertyPatternClauseSyntax",
            "QualifiedCrefSyntax",
            "SubpatternSyntax",
            "SwitchExpressionArmSyntax",
            "TypeCrefSyntax",
            "XmlCrefAttributeSyntax",
            "XmlElementEndTagSyntax",
            "XmlElementStartTagSyntax",
            "XmlNameAttributeSyntax",
            "XmlNameSyntax",
            "XmlPrefixSyntax",
            "XmlTextAttributeSyntax"
        };
}
