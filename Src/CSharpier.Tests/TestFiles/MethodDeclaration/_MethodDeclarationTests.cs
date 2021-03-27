using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class MethodDeclarationTests : BaseTest
    {
        [Test]
        public void BasicMethod()
        {
            this.RunTest("MethodDeclaration", "BasicMethod");
        }
        [Test]
        public void ExplicitlyInterfaceSpecifierMethod()
        {
            this.RunTest(
                "MethodDeclaration",
                "ExplicitlyInterfaceSpecifierMethod"
            );
        }
        [Test]
        public void LongMethodWithParameters()
        {
            this.RunTest("MethodDeclaration", "LongMethodWithParameters");
        }
        [Test]
        public void MethodComments()
        {
            this.RunTest("MethodDeclaration", "MethodComments");
        }
        [Test]
        public void MethodWithParameters()
        {
            this.RunTest("MethodDeclaration", "MethodWithParameters");
        }
        [Test]
        public void MethodWithStatements()
        {
            this.RunTest("MethodDeclaration", "MethodWithStatements");
        }
    }
}
