using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ThrowExpression
{
    public class ThrowExpressionTests : BaseTest
    {
        [Test]
        public void ThrowExpressions()
        {
            this.RunTest("ThrowExpression", "ThrowExpressions");
        }
    }
}
