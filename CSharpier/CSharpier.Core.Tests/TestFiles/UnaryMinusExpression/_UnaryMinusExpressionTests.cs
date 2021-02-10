using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
