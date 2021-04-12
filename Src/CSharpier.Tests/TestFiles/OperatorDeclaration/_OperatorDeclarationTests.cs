using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
