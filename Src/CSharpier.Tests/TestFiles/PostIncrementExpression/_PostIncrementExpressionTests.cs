using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.PostIncrementExpression
{
    public class PostIncrementExpressionTests : BaseTest
    {
        [Test]
        public void PostIncrementExpressions()
        {
            this.RunTest("PostIncrementExpression", "PostIncrementExpressions");
        }
    }
}
