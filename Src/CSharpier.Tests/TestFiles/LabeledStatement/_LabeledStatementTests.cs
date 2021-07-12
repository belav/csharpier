using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.LabeledStatement
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
