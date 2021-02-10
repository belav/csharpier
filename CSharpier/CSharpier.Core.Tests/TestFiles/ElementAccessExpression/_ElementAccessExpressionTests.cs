using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ElementAccessExpressionTests : BaseTest
    {
        [Test]
        public void BasicElementAccessExpression()
        {
            this.RunTest("ElementAccessExpression", "BasicElementAccessExpression");
        }
    }
}
