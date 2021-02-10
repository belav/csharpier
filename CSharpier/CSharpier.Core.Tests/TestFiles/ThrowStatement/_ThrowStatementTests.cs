using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ThrowStatementTests : BaseTest
    {
        [Test]
        public void BasicThrowStatement()
        {
            this.RunTest("ThrowStatement", "BasicThrowStatement");
        }
    }
}
