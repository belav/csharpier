using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class WhileStatementTests : BaseTest
    {
        [Test]
        public void BasicWhileStatement()
        {
            this.RunTest("WhileStatement", "BasicWhileStatement");
        }
    }
}
