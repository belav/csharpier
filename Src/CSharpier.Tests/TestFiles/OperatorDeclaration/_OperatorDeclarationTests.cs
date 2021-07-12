using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.OperatorDeclaration
{
    public class OperatorDeclarationTests : BaseTest
    {
        [Test]
        public void OperatorDeclarations()
        {
            this.RunTest("OperatorDeclaration", "OperatorDeclarations");
        }
    }
}
