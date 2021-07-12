using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.FieldDeclaration
{
    public class FieldDeclarationTests : BaseTest
    {
        [Test]
        public void FieldDeclarations()
        {
            this.RunTest("FieldDeclaration", "FieldDeclarations");
        }
    }
}
