using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class NamespaceDeclarationTests : BaseTest
    {
        [Test]
        public void EmptyNamespace()
        {
            this.RunTest("NamespaceDeclaration", "EmptyNamespace");
        }
        [Test]
        public void NamespaceComments()
        {
            this.RunTest("NamespaceDeclaration", "NamespaceComments");
        }
        [Test]
        public void NamespaceWithMultipleClasses()
        {
            this.RunTest(
                "NamespaceDeclaration",
                "NamespaceWithMultipleClasses"
            );
        }
        [Test]
        public void NamespaceWithUsingAndClass()
        {
            this.RunTest("NamespaceDeclaration", "NamespaceWithUsingAndClass");
        }
    }
}
