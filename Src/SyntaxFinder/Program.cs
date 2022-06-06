using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxFinder;

// Customize this in some way to find variations of syntax
// csharpier-repos isn't a huge result set, but it can give you
// some idea what syntax people prefer
// make sure csharpier-repos is on main before running
public class Program
{
    public static void Main()
    {
        var files = Directory.EnumerateFiles(
            "C:\\projects\\csharpier-repos",
            //@"C:\projects\insite-commerce-csharpier\Legacy\Core\Insite.Core.Interfaces\Data",
            "*.cs",
            SearchOption.AllDirectories
        );

        Parallel.ForEach(
            files,
            file =>
            {
                if (Ignored.Is(file))
                {
                    return;
                }

                var contents = File.ReadAllText(file);
                var tree = CSharpSyntaxTree.ParseText(contents);

                var customWalker = new CustomWalker(file);
                customWalker.Visit(tree.GetRoot());
            }
        );

        foreach (var entry in CustomWalker.MembersInType)
        {
            Console.WriteLine(entry.Key);
            foreach (var member in entry.Value.OrderBy(o => o))
            {
                Console.WriteLine("    " + member);
            }
        }

        if (CustomWalker.Matching > CustomWalker.Total)
        {
            Console.WriteLine("Matching was > than Total, so you did something wrong.");
        }

        WriteResult("Matching", CustomWalker.Matching.ToString("n0"));
        WriteResult("Total", CustomWalker.Total.ToString("n0"));
        WriteResult(
            "Percent",
            (Convert.ToDecimal(CustomWalker.Matching) / CustomWalker.Total * 100).ToString("n")
                + "%"
        );
    }

    private static void WriteResult(string label, string value)
    {
        Console.WriteLine((label + ":").PadRight(20) + value.PadLeft(10));
    }
}

public class CustomWalker : CSharpSyntaxWalker
{
    public static readonly ConcurrentDictionary<string, List<string>> MembersInType = new();
    public static int Total;
    public static int Matching;
    private readonly string file;
    private bool wroteFile;
    private readonly int maxCodeWrites = 250;
    private int codeWrites = 0;

    public CustomWalker(string file)
    {
        this.file = file;
    }

    // public override void VisitCompilationUnit(CompilationUnitSyntax node)
    // {
    //     this.VisitType(node, node.Members);
    //     base.VisitCompilationUnit(node);
    // }

    private void VisitType(CSharpSyntaxNode node, SyntaxList<MemberDeclarationSyntax> members)
    {
        foreach (var member in members)
        {
            MembersInType.AddOrUpdate(
                node.GetType().Name,
                new List<string>(),
                (key, list) =>
                {
                    if (!list.Contains(member.GetType().Name))
                    {
                        list.Add(member.GetType().Name);
                    }

                    return list;
                }
            );
        }
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        // if (
        //     (
        //         node.Parent is TypeDeclarationSyntax typeDeclarationSyntax
        //         && node == typeDeclarationSyntax.Members.First()
        //     ) || node.GetLeadingTrivia().Any(o => o.IsComment() || node.AttributeLists.Any())
        // )
        // {
        //     base.VisitMethodDeclaration(node);
        //     return;
        // }
        //
        // Interlocked.Increment(ref Total);
        // this.WriteCode(node.Parent!);
        //
        // if (node.GetLeadingTrivia().Any(o => o.Kind() is SyntaxKind.EndOfLineTrivia))
        // {
        //     Interlocked.Increment(ref Matching);
        // }

        base.VisitMethodDeclaration(node);
    }

    public override void VisitListPattern(ListPatternSyntax node)
    {
        Interlocked.Increment(ref Total);
        this.WriteCode(node.Parent!);

        base.VisitListPattern(node);
    }

    private void WriteCode(SyntaxNode syntaxNode)
    {
        if (this.codeWrites < this.maxCodeWrites)
        {
            Interlocked.Increment(ref this.codeWrites);
            Console.WriteLine(syntaxNode.SyntaxTree.GetText().ToString(syntaxNode.Span));
        }
    }

    private void WriteFilePath()
    {
        if (!this.wroteFile)
        {
            Console.WriteLine(this.file);
            this.wroteFile = true;
        }
    }

    private static bool IsMultiline(SyntaxNode syntaxNode)
    {
        var lineSpan = syntaxNode.SyntaxTree.GetLineSpan(syntaxNode.Span);
        return lineSpan.StartLinePosition.Line != lineSpan.EndLinePosition.Line;
    }
}

public static class SyntaxTriviaExtensions
{
    public static bool IsComment(this SyntaxTrivia syntaxTrivia)
    {
        return syntaxTrivia.Kind()
            is SyntaxKind.SingleLineCommentTrivia
                or SyntaxKind.MultiLineCommentTrivia
                or SyntaxKind.SingleLineDocumentationCommentTrivia
                or SyntaxKind.MultiLineDocumentationCommentTrivia;
    }
}

public static class Ignored
{
    public static bool Is(string file)
    {
        return ignored.Any(file.Replace("\\", "/").Contains);
    }

    private static string[] ignored = new[]
    {
        "roslyn/src/Compilers/Test/Core/Assert/ConditionalFactAttribute.cs",
        "roslyn/src/Compilers/Test/Core/Compilation/RuntimeUtilities.cs",
        "runtime/src/libraries/System.Net.Primitives/tests/FunctionalTests/SocketAddressTest.cs",
        "runtime/src/libraries/System.Security.Cryptography.Pkcs/tests/SignedCms/SignedDocuments.cs",
        "aspnetcore/src/ProjectTemplates/Web.ProjectTemplates/content/StarterWeb-CSharp/Controllers/HomeController.cs",
        "aspnetcore/src/ProjectTemplates/Web.ProjectTemplates/content/WebApi-CSharp/Controllers/WeatherForecastController.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/EmptyExplicitExpression.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/EmptyImplicitExpression.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/EmptyImplicitExpressionInCode.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/EmptyImplicitExpressionInCode.Tabs.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/ExplicitExpressionAtEOF.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/HelpersMissingCloseParen.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/HelpersMissingOpenBrace.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/HiddenSpansInCode.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/HelpersMissingOpenParen.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/ImplicitExpressionAtEOF.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/Inherits.Designtime.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/Inherits.Runtime.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/OpenedIf.DesignTime.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/OpenedIf.DesignTime.Tabs.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/ParserError.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/RazorComments.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/RazorComments.DesignTime.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/Sections.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/UnfinishedExpressionInCode.cs",
        "AspNetWebStack/test/System.Web.Razor.Test/TestFiles/CodeGenerator/CS/Output/UnfinishedExpressionInCode.Tabs.cs",
        "runtime/src/libraries/System.Text.Encoding.CodePages/src/System/Text/EncodingTable.Data.cs",
        "runtime/src/libraries/System.Text.Json/tests/TrimmingTests/Collections/ICollection.cs",
        "runtime/src/tests/JIT/jit64/opt/cse/HugeArray.cs",
        "runtime/src/tests/JIT/jit64/opt/rngchk/RngchkStress2.cs",
        "runtime/src/tests/JIT/jit64/opt/cse/HugeArray1.cs",
        "runtime/src/tests/JIT/Methodical/largeframes/skip2/skippage2.cs",
        "runtime/src/tests/JIT/Methodical/largeframes/skip6/skippage6.cs",
        "runtime/src/tests/JIT/Performance/CodeQuality/Bytemark/neural-dat.cs",
        "runtime/src/tests/JIT/Regression/JitBlue/GitHub_10215/GitHub_10215.cs",
        "aspnetcore/src/ProjectTemplates/Web.ProjectTemplates/content/ComponentsWebAssembly-CSharp/Server/Controllers/WeatherForecastController.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ComplexTagHelpers_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ComplexTagHelpers_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EmptyAttributeTagHelpers_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EmptyAttributeTagHelpers_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EmptyExplicitExpression_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EmptyImplicitExpressionInCode_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EmptyImplicitExpression_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExplicitExpressionAtEOF_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/HiddenSpansInCode_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/HiddenSpansInCode_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ImplicitExpressionAtEOF_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/IncompleteDirectives_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/OpenedIf_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ParserError_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/OpenedIf_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ParserError_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Sections_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Sections_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TransitionsInTagHelperAttributes_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/UnfinishedExpressionInCode_Runtime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/UnfinishedExpressionInCode_DesignTime.codegen.cs",
        "aspnetcore/src/Razor/Microsoft.AspNetCore.Razor.Language/test/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TransitionsInTagHelperAttributes_Runtime.codegen.cs",
        "runtime/src/tests/JIT/jit64/opt/cse/hugeSimpleExpr1.cs",
        "runtime/src/tests/JIT/jit64/opt/cse/HugeField1.cs",
        "runtime/src/tests/JIT/jit64/opt/cse/HugeField2.cs",
        "roslyn/src/Compilers/Test/Resources/Core/SymbolsTests/Metadata/public-and-private.cs",
        "runtime/src/tests/JIT/jit64/opt/cse/hugeexpr1.cs",
        "runtime/src/tests/Loader/classloader/generics/Instantiation/Nesting/NestedGenericClasses.cs",
        "runtime/src/tests/Loader/classloader/generics/Instantiation/Nesting/NestedGenericTypesMix.cs",
        "runtime/src/tests/Loader/classloader/generics/Instantiation/Nesting/NestedGenericStructs.cs",
    };
}
