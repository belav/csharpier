using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class LocalFunctionStatementTests : BaseTest
    {
        [Test]
        public void BasicLocalFunctionStatement()
        {
            this.RunTest(
                "LocalFunctionStatement",
                "BasicLocalFunctionStatement"
            );
        }
    }
}
