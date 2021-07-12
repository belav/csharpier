using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.UsingStatement
{
    public class UsingStatementTests : BaseTest
    {
        [Test]
        public void UsingStatements()
        {
            this.RunTest("UsingStatement", "UsingStatements");
        }
    }
}
