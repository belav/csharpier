using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ReturnStatementTests : BaseTest
    {
        [Test]
        public void EmptyReturn()
        {
            this.RunTest("ReturnStatement", "EmptyReturn");
        }
    }
}
