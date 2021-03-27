using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ElementBindingExpressionTests : BaseTest
    {
        [Test]
        public void BasicElementBindingExpression()
        {
            this.RunTest(
                "ElementBindingExpression",
                "BasicElementBindingExpression"
            );
        }
    }
}
