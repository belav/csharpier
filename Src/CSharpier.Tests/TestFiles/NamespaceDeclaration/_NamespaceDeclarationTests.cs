using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.NamespaceDeclaration
{
    public class NamespaceDeclarationTests : BaseTest
    {
        [Test]
        public void NamespaceDeclarations()
        {
            this.RunTest("NamespaceDeclaration", "NamespaceDeclarations");
        }
    }
}
