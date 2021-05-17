using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class StringLiteralTests : BaseTest
    {
        [Test]
        public void VerbatimStrings()
        {
            this.RunTest("StringLiteral", "VerbatimStrings");
        }
    }
}
