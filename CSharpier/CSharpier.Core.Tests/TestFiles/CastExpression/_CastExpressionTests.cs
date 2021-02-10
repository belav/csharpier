using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class CastExpressionTests : BaseTest
    {
        [Test]
        public void BasicCastExpression()
        {
            this.RunTest("CastExpression", "BasicCastExpression");
        }
    }
}
