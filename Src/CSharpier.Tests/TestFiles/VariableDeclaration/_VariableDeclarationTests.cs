using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.VariableDeclaration
{
    public class VariableDeclarationTests : BaseTest
    {
        [Test]
        public void VariableDeclarations()
        {
            this.RunTest("VariableDeclaration", "VariableDeclarations");
        }
    }
}
