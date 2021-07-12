using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.UnaryPlusExpression
{
    public class UnaryPlusExpressionTests : BaseTest
    {
        [Test]
        public void UnaryPlusExpressions()
        {
            this.RunTest("UnaryPlusExpression", "UnaryPlusExpressions");
        }
    }
}
