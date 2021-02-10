using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ContinueStatementTests : BaseTest
    {
        [Test]
        public void BasicContinueStatement()
        {
            this.RunTest("ContinueStatement", "BasicContinueStatement");
        }
    }
}
