using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class UnaryMinusExpressionTests : BaseTest
    {
        [Test]
        public void UnaryMinusExpressions()
        {
            this.RunTest("UnaryMinusExpression", "UnaryMinusExpressions");
        }
    }
}
