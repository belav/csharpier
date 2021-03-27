using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class InterpolatedStringExpressionTests : BaseTest
    {
        [Test]
        public void InterpolatedStringExpressions()
        {
            this.RunTest(
                "InterpolatedStringExpression",
                "InterpolatedStringExpressions"
            );
        }
    }
}
