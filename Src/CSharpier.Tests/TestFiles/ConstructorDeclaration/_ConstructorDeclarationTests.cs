using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ConstructorDeclarationTests : BaseTest
    {
        [Test]
        public void BasicConstructorDeclaration()
        {
            this.RunTest(
                "ConstructorDeclaration",
                "BasicConstructorDeclaration"
            );
        }
        [Test]
        public void ConstructorDeclarationComments()
        {
            this.RunTest(
                "ConstructorDeclaration",
                "ConstructorDeclarationComments"
            );
        }
        [Test]
        public void ConstructorDeclarationWithInitializer()
        {
            this.RunTest(
                "ConstructorDeclaration",
                "ConstructorDeclarationWithInitializer"
            );
        }
        [Test]
        public void ConstructorWithParameters()
        {
            this.RunTest("ConstructorDeclaration", "ConstructorWithParameters");
        }
        [Test]
        public void LongConstructor()
        {
            this.RunTest("ConstructorDeclaration", "LongConstructor");
        }
    }
}
