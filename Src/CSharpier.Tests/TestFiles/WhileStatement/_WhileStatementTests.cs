using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class WhileStatementTests : BaseTest
    {
        [Test]
        public void WhileStatements()
        {
            this.RunTest("WhileStatement", "WhileStatements");
        }
    }
}
