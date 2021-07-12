using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ImplicitStackAllocArrayCreationExpression
{
    public class ImplicitStackAllocArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void ImplicitStackAllocArrayCreationExpressions()
        {
            this.RunTest(
                "ImplicitStackAllocArrayCreationExpression",
                "ImplicitStackAllocArrayCreationExpressions"
            );
        }
    }
}
