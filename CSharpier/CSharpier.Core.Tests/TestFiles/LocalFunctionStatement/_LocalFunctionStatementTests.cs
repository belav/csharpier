using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class LocalFunctionStatementTests : BaseTest
    {
        [Test]
        public void BasicLocalFunctionStatement()
        {
            this.RunTest("LocalFunctionStatement", "BasicLocalFunctionStatement");
        }
    }
}
