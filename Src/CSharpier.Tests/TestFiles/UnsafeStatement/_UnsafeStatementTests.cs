using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.UnsafeStatement
{
    public class UnsafeStatementTests : BaseTest
    {
        [Test]
        public void UnsafeStatements()
        {
            this.RunTest("UnsafeStatement", "UnsafeStatements");
        }
    }
}
