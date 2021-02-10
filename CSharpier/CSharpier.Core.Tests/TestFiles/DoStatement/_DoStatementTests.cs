using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class DoStatementTests : BaseTest
    {
        [Test]
        public void BasicDoStatement()
        {
            this.RunTest("DoStatement", "BasicDoStatement");
        }
    }
}
