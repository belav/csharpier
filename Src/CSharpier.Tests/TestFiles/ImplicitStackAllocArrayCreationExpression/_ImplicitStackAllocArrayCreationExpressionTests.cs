using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
