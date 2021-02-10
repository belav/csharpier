using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class RangeExpressionTests : BaseTest
    {
        [Test]
        public void BasicRangeExpression()
        {
            this.RunTest("RangeExpression", "BasicRangeExpression");
        }
    }
}
