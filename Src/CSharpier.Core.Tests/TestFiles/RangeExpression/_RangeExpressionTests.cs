using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
