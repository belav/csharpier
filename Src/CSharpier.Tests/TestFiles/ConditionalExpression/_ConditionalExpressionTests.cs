using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ConditionalExpression
{
    public class ConditionalExpressionTests : BaseTest
    {
        [Test]
        public void ConditionalExpressions()
        {
            this.RunTest("ConditionalExpression", "ConditionalExpressions");
        }
    }
}
