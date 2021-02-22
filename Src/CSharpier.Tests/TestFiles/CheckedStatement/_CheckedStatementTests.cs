using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class CheckedStatementTests : BaseTest
    {
        [Test]
        public void BasicCheckedStatement()
        {
            this.RunTest("CheckedStatement", "BasicCheckedStatement");
        }
    }
}
