using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class UnsafeStatementTests : BaseTest
    {
        [Test]
        public void BasicUnsafeStatement()
        {
            this.RunTest("UnsafeStatement", "BasicUnsafeStatement");
        }
    }
}
