using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
