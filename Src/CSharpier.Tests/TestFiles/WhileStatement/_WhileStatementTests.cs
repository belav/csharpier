using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
