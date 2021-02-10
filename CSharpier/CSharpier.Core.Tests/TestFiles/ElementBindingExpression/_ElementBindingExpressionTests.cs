using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ElementBindingExpressionTests : BaseTest
    {
        [Test]
        public void BasicElementBindingExpression()
        {
            this.RunTest("ElementBindingExpression", "BasicElementBindingExpression");
        }
    }
}
