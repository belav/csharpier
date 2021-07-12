using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.YieldReturnStatement
{
    public class YieldReturnStatementTests : BaseTest
    {
        [Test]
        public void YieldReturnStatements()
        {
            this.RunTest("YieldReturnStatement", "YieldReturnStatements");
        }
    }
}
