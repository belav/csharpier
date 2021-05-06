using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class AnonymousMethodExpressionTests : BaseTest
    {
        [Test]
        public void AnonymousMethodExpressions()
        {
            this.RunTest("AnonymousMethodExpression", "AnonymousMethodExpressions");
        }
    }
}
