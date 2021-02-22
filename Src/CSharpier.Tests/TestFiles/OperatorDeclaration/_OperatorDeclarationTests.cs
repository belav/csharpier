using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class OperatorDeclarationTests : BaseTest
    {
        [Test]
        public void BasicOperatorDeclaration()
        {
            this.RunTest("OperatorDeclaration", "BasicOperatorDeclaration");
        }
    }
}
