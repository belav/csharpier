using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.InterpolatedStringExpression
{
    public class InterpolatedStringExpressionTests : BaseTest
    {
        [Test]
        public void InterpolatedStringExpressions()
        {
            this.RunTest("InterpolatedStringExpression", "InterpolatedStringExpressions");
        }
    }
}
