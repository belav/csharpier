using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AnonymousMethodExpression
{
    public class AnonymousMethodExpressionTests : BaseTest
    {
        [Test]
        public void AnonymousMethodExpressions()
        {
            this.RunTest("AnonymousMethodExpression", "AnonymousMethodExpressions");
        }
    }
}
