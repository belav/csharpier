using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class IfStatementTests : BaseTest
    {
        [Test]
        public void AndIfStatement()
        {
            this.RunTest("IfStatement", "AndIfStatement");
        }
        [Test]
        public void BasicIfStatement()
        {
            this.RunTest("IfStatement", "BasicIfStatement");
        }
        [Test]
        public void EqualsIfStatement()
        {
            this.RunTest("IfStatement", "EqualsIfStatement");
        }
        [Test]
        public void IfStatementWithNoBraces()
        {
            this.RunTest("IfStatement", "IfStatementWithNoBraces");
        }
        [Test]
        public void LogicalNotIfStatement()
        {
            this.RunTest("IfStatement", "LogicalNotIfStatement");
        }
        [Test]
        public void NotEqualsIfStatement()
        {
            this.RunTest("IfStatement", "NotEqualsIfStatement");
        }
        [Test]
        public void OrIfStatement()
        {
            this.RunTest("IfStatement", "OrIfStatement");
        }
        [Test]
        public void ParenthesizedIfStatement()
        {
            this.RunTest("IfStatement", "ParenthesizedIfStatement");
        }
    }
}
