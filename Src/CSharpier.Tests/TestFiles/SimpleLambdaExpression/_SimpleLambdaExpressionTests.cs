using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.SimpleLambdaExpression
{
    public class SimpleLambdaExpressionTests : BaseTest
    {
        [Test]
        public void SimpleLambdaExpressions()
        {
            this.RunTest("SimpleLambdaExpression", "SimpleLambdaExpressions");
        }
    }
}
