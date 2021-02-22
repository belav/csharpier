using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void ArrayCreationWithInitializer()
        {
            this.RunTest(
                "ArrayCreationExpression",
                "ArrayCreationWithInitializer");
        }
        [Test]
        public void BasicArrayCreationExpression()
        {
            this.RunTest(
                "ArrayCreationExpression",
                "BasicArrayCreationExpression");
        }
    }
}
