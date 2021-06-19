using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ParenthesizedExpressionTests : BaseTest
    {
        [Test]
        public void ParenthesizedExpressions()
        {
            this.RunTest("ParenthesizedExpression", "ParenthesizedExpressions");
        }
    }
}
