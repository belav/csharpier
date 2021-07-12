using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.InvocationExpression
{
    public class InvocationExpressionTests : BaseTest
    {
        [Test]
        public void InvocationExpressions()
        {
            this.RunTest("InvocationExpression", "InvocationExpressions");
        }
    }
}
