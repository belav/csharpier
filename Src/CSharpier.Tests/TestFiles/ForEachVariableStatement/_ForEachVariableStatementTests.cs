using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ForEachVariableStatementTests : BaseTest
    {
        [Test]
        public void BasicForEachVariableStatement()
        {
            this.RunTest(
                "ForEachVariableStatement",
                "BasicForEachVariableStatement"
            );
        }
    }
}
