using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class CastExpressionTests : BaseTest
    {
        [Test]
        public void CastExpressions()
        {
            this.RunTest("CastExpression", "CastExpressions");
        }
    }
}
