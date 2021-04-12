using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
