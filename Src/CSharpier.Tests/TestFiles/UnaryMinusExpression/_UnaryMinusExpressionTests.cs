using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class UnaryMinusExpressionTests : BaseTest
    {
        [Test]
        public void BasicUnaryMinusExpression()
        {
            this.RunTest("UnaryMinusExpression", "BasicUnaryMinusExpression");
        }
    }
}
