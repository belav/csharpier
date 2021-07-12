using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.BinaryExpression
{
    public class BinaryExpressionTests : BaseTest
    {
        [Test]
        public void BinaryExpressions()
        {
            this.RunTest("BinaryExpression", "BinaryExpressions");
        }
    }
}
