using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ReturnStatement
{
    public class ReturnStatementTests : BaseTest
    {
        [Test]
        public void ReturnStatements()
        {
            this.RunTest("ReturnStatement", "ReturnStatements");
        }
    }
}
