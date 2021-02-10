using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class UsingStatementTests : BaseTest
    {
        [Test]
        public void BasicUsingStatement()
        {
            this.RunTest("UsingStatement", "BasicUsingStatement");
        }
        [Test]
        public void NestedUsingStatement()
        {
            this.RunTest("UsingStatement", "NestedUsingStatement");
        }
        [Test]
        public void UsingStatementWithExpression()
        {
            this.RunTest("UsingStatement", "UsingStatementWithExpression");
        }
        [Test]
        public void UsingStatementWithNoBody()
        {
            this.RunTest("UsingStatement", "UsingStatementWithNoBody");
        }
    }
}
