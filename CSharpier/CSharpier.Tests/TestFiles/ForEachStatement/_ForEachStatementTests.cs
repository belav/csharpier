using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ForEachStatementTests : BaseTest
    {
        [Test]
        public void BasicForEachStatement()
        {
            this.RunTest("ForEachStatement", "BasicForEachStatement");
        }
    }
}
