using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class TryStatementTests : BaseTest
    {
        [Test]
        public void TryStatements()
        {
            this.RunTest("TryStatement", "TryStatements");
        }
    }
}
