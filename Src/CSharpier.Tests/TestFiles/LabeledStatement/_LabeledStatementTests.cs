using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class LabeledStatementTests : BaseTest
    {
        [Test]
        public void LabeledStatements()
        {
            this.RunTest("LabeledStatement", "LabeledStatements");
        }
    }
}
