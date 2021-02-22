using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ForEachVariableStatementTests : BaseTest
    {
        [Test]
        public void BasicForEachVariableStatement()
        {
            this.RunTest(
                "ForEachVariableStatement",
                "BasicForEachVariableStatement");
        }
    }
}
