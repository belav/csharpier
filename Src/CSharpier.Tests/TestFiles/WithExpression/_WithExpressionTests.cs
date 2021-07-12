using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.WithExpression
{
    public class WithExpressionTests : BaseTest
    {
        [Test]
        public void WithExpressions()
        {
            this.RunTest("WithExpression", "WithExpressions");
        }
    }
}
