using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace ParserTests
{
    public class Tests
    {
        [Test]
        public void TestingComments()
        {
            var rootNode = CSharpSyntaxTree.ParseText(@"// class comment
public class ClassName {}").GetRoot();

            var classDeclaration = (rootNode as CompilationUnitSyntax).Members[0] as ClassDeclarationSyntax;
            var trivia = classDeclaration.Modifiers[0].LeadingTrivia.First();
            var node = trivia.GetStructure();
            Assert.AreEqual("// class comment", trivia.ToString());
        }
    }
}