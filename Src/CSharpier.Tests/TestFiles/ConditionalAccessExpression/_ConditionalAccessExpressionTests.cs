using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ConditionalAccessExpression
{
    public class ConditionalAccessExpressionTests : BaseTest
    {
        [Test]
        public void ConditionalAccessExpressions()
        {
            this.RunTest("ConditionalAccessExpression", "ConditionalAccessExpressions");
        }
    }
}
