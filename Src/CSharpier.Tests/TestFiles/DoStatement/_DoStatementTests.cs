using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DoStatementTests : BaseTest
    {
        [Test]
        public void DoStatements()
        {
            this.RunTest("DoStatement", "DoStatements");
        }
    }
}
