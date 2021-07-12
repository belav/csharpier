using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.CheckedStatement
{
    public class CheckedStatementTests : BaseTest
    {
        [Test]
        public void CheckedStatements()
        {
            this.RunTest("CheckedStatement", "CheckedStatements");
        }
    }
}
