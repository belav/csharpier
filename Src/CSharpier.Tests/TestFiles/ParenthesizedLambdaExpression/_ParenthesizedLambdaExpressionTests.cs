using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ParenthesizedLambdaExpressionTests : BaseTest
    {
        [Test]
        public void AsyncAwaitParaenthesizedLambdaExpression()
        {
            this.RunTest(
                "ParenthesizedLambdaExpression",
                "AsyncAwaitParaenthesizedLambdaExpression"
            );
        }
        [Test]
        public void BasicParenthesizedLambdaExpression()
        {
            this.RunTest(
                "ParenthesizedLambdaExpression",
                "BasicParenthesizedLambdaExpression"
            );
        }
    }
}
