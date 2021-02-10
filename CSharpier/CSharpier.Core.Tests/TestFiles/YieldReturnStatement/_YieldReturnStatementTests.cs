using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
