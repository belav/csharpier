using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.CoalesceExpression
{
    public class CoalesceExpressionTests : BaseTest
    {
        [Test]
        public void CoalesceExpressions()
        {
            this.RunTest("CoalesceExpression", "CoalesceExpressions");
        }
    }
}
