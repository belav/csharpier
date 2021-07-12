using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AwaitExpression
{
    public class AwaitExpressionTests : BaseTest
    {
        [Test]
        public void AwaitExpressions()
        {
            this.RunTest("AwaitExpression", "AwaitExpressions");
        }
    }
}
