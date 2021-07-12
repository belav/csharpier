using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.UnaryMinusExpression
{
    public class UnaryMinusExpressionTests : BaseTest
    {
        [Test]
        public void UnaryMinusExpressions()
        {
            this.RunTest("UnaryMinusExpression", "UnaryMinusExpressions");
        }
    }
}
