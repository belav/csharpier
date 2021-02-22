using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
