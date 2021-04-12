using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
