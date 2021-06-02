using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
