using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class UnaryPlusExpressionTests : BaseTest
    {
        [Test]
        public void BasicUnaryPlusExpression()
        {
            this.RunTest("UnaryPlusExpression", "BasicUnaryPlusExpression");
        }
    }
}
