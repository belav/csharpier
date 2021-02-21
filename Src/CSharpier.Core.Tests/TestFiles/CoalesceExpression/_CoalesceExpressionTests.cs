using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class CoalesceExpressionTests : BaseTest
    {
        [Test]
        public void BasicCoalesceExpression()
        {
            this.RunTest("CoalesceExpression", "BasicCoalesceExpression");
        }
    }
}
