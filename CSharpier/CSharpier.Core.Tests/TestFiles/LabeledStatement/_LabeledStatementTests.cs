using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class LabeledStatementTests : BaseTest
    {
        [Test]
        public void BasicLabeledStatement()
        {
            this.RunTest("LabeledStatement", "BasicLabeledStatement");
        }
    }
}
