using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class RefExpressionTests : BaseTest
    {
        [Test]
        public void BasicRefExpression()
        {
            this.RunTest("RefExpression", "BasicRefExpression");
        }
    }
}
