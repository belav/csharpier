using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.PreIncrementExpression
{
    public class PreIncrementExpressionTests : BaseTest
    {
        [Test]
        public void PreIncrementExpressions()
        {
            this.RunTest("PreIncrementExpression", "PreIncrementExpressions");
        }
    }
}
