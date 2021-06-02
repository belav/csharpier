using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ElementBindingExpressionTests : BaseTest
    {
        [Test]
        public void ElementBindingExpressions()
        {
            this.RunTest("ElementBindingExpression", "ElementBindingExpressions");
        }
    }
}
