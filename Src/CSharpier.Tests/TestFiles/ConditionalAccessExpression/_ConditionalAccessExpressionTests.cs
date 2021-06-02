using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ConditionalAccessExpressionTests : BaseTest
    {
        [Test]
        public void ConditionalAccessExpressions()
        {
            this.RunTest("ConditionalAccessExpression", "ConditionalAccessExpressions");
        }
    }
}
