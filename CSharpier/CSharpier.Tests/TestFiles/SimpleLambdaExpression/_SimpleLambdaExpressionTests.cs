using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class SimpleLambdaExpressionTests : BaseTest
    {
        [Test]
        public void BasicSimpleLambdaExpression()
        {
            this.RunTest("SimpleLambdaExpression", "BasicSimpleLambdaExpression");
        }
    }
}
