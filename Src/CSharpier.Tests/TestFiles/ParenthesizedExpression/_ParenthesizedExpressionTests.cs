using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ParenthesizedExpression
{
    public class ParenthesizedExpressionTests : BaseTest
    {
        [Test]
        public void ParenthesizedExpressions()
        {
            this.RunTest("ParenthesizedExpression", "ParenthesizedExpressions");
        }
    }
}
