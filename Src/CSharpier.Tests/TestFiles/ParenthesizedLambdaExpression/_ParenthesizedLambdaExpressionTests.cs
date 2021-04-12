using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ParenthesizedLambdaExpressionTests : BaseTest
    {
        [Test]
        public void ParenthesizedLambdaExpressions()
        {
            this.RunTest(
                "ParenthesizedLambdaExpression",
                "ParenthesizedLambdaExpressions"
            );
        }
    }
}
