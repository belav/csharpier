using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ForStatementTests : BaseTest
    {
        [Test]
        public void ForStatements()
        {
            this.RunTest("ForStatement", "ForStatements");
        }
    }
}
