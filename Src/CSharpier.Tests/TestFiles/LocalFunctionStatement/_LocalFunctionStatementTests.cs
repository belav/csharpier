using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.LocalFunctionStatement
{
    public class LocalFunctionStatementTests : BaseTest
    {
        [Test]
        public void LocalFunctionStatements()
        {
            this.RunTest("LocalFunctionStatement", "LocalFunctionStatements");
        }
    }
}
