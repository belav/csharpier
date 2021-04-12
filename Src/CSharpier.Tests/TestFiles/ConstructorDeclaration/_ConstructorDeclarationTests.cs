using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ConstructorDeclarationTests : BaseTest
    {
        [Test]
        public void ConstructorDeclarations()
        {
            this.RunTest("ConstructorDeclaration", "ConstructorDeclarations");
        }
    }
}
