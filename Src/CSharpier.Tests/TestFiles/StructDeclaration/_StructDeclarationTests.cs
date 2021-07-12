using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.StructDeclaration
{
    public class StructDeclarationTests : BaseTest
    {
        [Test]
        public void StructDeclarations()
        {
            this.RunTest("StructDeclaration", "StructDeclarations");
        }
    }
}
