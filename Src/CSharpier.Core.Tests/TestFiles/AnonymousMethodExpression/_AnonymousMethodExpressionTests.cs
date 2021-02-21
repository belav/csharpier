using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class AnonymousMethodExpressionTests : BaseTest
    {
        [Test]
        public void AsyncDelegateAnonymousMethodExpression()
        {
            this.RunTest("AnonymousMethodExpression", "AsyncDelegateAnonymousMethodExpression");
        }
        [Test]
        public void BasicAnonymousMethodExpression()
        {
            this.RunTest("AnonymousMethodExpression", "BasicAnonymousMethodExpression");
        }
    }
}
