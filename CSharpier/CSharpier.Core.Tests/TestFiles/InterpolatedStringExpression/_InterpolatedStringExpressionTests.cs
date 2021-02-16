using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class InterpolatedStringExpressionTests : BaseTest
    {
        [Test]
        public void BasicInterpolatedStringExpression()
        {
            this.RunTest("InterpolatedStringExpression", "BasicInterpolatedStringExpression");
        }
        [Test]
        public void Interpolation()
        {
            this.RunTest("InterpolatedStringExpression", "Interpolation");
        }
        [Test]
        public void InterpolationShouldNotBreakLines()
        {
            this.RunTest("InterpolatedStringExpression", "InterpolationShouldNotBreakLines");
        }
        [Test]
        public void InterpolationWithAlignmentAndFormat()
        {
            this.RunTest("InterpolatedStringExpression", "InterpolationWithAlignmentAndFormat");
        }
    }
}
