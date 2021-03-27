using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ImplicitStackAllocArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void BasicImplicitStackAllocArrayCreationExpression()
        {
            this.RunTest(
                "ImplicitStackAllocArrayCreationExpression",
                "BasicImplicitStackAllocArrayCreationExpression"
            );
        }
    }
}
