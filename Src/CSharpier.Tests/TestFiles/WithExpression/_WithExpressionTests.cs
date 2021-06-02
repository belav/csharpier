using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class WithExpressionTests : BaseTest
    {
        [Test]
        public void WithExpressions()
        {
            this.RunTest("WithExpression", "WithExpressions");
        }
    }
}
