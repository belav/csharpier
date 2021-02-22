using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class FixedStatementTests : BaseTest
    {
        [Test]
        public void BasicFixedStatement()
        {
            this.RunTest("FixedStatement", "BasicFixedStatement");
        }
    }
}
