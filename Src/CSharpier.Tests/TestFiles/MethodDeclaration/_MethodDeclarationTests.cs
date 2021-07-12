using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.MethodDeclaration
{
    public class MethodDeclarationTests : BaseTest
    {
        [Test]
        public void MethodDeclarations()
        {
            this.RunTest("MethodDeclaration", "MethodDeclarations");
        }
    }
}
