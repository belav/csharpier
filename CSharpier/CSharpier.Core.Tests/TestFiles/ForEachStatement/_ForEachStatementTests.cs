using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
