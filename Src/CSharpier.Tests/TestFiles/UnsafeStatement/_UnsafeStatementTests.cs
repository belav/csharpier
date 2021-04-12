using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
