using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
