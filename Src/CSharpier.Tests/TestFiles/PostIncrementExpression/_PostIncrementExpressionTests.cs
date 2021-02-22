using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class PostIncrementExpressionTests : BaseTest
    {
        [Test]
        public void BasicPostIncrementExpression()
        {
            this.RunTest(
                "PostIncrementExpression",
                "BasicPostIncrementExpression");
        }
    }
}
