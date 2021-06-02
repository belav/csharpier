using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ForEachVariableStatementTests : BaseTest
    {
        [Test]
        public void ForEachVariableStatements()
        {
            this.RunTest("ForEachVariableStatement", "ForEachVariableStatements");
        }
    }
}
