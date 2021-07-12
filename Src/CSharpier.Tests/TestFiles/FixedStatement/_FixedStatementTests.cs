using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.FixedStatement
{
    public class FixedStatementTests : BaseTest
    {
        [Test]
        public void FixedStatements()
        {
            this.RunTest("FixedStatement", "FixedStatements");
        }
    }
}
