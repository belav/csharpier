using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class InvocationExpressionTests : BaseTest
    {
        [Test]
        public void InvocationExpressions()
        {
            this.RunTest(
                "InvocationExpression",
                "InvocationExpressions");
        }
    }
}
