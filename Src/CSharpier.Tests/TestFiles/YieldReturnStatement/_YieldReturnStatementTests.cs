using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class YieldReturnStatementTests : BaseTest
    {
        [Test]
        public void BasicYieldReturnStatement()
        {
            this.RunTest("YieldReturnStatement", "BasicYieldReturnStatement");
        }
    }
}
