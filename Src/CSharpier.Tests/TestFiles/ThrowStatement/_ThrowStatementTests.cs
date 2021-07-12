using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ThrowStatement
{
    public class ThrowStatementTests : BaseTest
    {
        [Test]
        public void ThrowStatements()
        {
            this.RunTest("ThrowStatement", "ThrowStatements");
        }
    }
}
