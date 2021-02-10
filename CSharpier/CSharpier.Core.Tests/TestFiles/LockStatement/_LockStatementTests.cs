using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class LockStatementTests : BaseTest
    {
        [Test]
        public void BasicLockStatement()
        {
            this.RunTest("LockStatement", "BasicLockStatement");
        }
    }
}
