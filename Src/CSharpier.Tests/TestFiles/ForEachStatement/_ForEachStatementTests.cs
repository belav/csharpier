using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ForEachStatementTests : BaseTest
    {
        [Test]
        public void ForEachStatements()
        {
            this.RunTest("ForEachStatement", "ForEachStatements");
        }
    }
}
