using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.RefExpression
{
    public class RefExpressionTests : BaseTest
    {
        [Test]
        public void RefExpressions()
        {
            this.RunTest("RefExpression", "RefExpressions");
        }
    }
}
