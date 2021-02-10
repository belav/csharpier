using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ForStatementTests : BaseTest
    {
        [Test]
        public void BasicForStatement()
        {
            this.RunTest("ForStatement", "BasicForStatement");
        }
        [Test]
        public void EmptyForStatement()
        {
            this.RunTest("ForStatement", "EmptyForStatement");
        }
        [Test]
        public void ForStatementNoBraces()
        {
            this.RunTest("ForStatement", "ForStatementNoBraces");
        }
    }
}
