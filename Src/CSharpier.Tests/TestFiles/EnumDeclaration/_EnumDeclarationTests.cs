using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.EnumDeclaration
{
    public class EnumDeclarationTests : BaseTest
    {
        [Test]
        public void EnumDeclarations()
        {
            this.RunTest("EnumDeclaration", "EnumDeclarations");
        }
    }
}
