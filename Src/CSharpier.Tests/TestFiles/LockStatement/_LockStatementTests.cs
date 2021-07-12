using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.LockStatement
{
    public class LockStatementTests : BaseTest
    {
        [Test]
        public void LockStatements()
        {
            this.RunTest("LockStatement", "LockStatements");
        }
    }
}
