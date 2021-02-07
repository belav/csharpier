using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class PreprocessorStatementsTests : BaseTest
    {
        [Test]
        public void IfDirectiveEmptyBlock()
        {
            this.RunTest("PreprocessorStatements", "IfDirectiveEmptyBlock");
        }
    }
}
