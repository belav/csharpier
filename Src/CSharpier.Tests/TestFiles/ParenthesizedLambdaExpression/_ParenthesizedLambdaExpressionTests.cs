using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ParenthesizedLambdaExpression
{
    public class ParenthesizedLambdaExpressionTests : BaseTest
    {
        [Test]
        public void ParenthesizedLambdaExpressions()
        {
            this.RunTest("ParenthesizedLambdaExpression", "ParenthesizedLambdaExpressions");
        }
    }
}
