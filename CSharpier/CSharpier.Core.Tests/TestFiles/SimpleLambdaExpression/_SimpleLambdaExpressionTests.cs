using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
