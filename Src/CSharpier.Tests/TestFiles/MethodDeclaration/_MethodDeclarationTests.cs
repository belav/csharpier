using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
