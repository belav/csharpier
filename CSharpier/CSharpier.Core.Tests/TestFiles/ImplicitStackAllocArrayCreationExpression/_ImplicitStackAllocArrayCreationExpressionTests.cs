using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ImplicitStackAllocArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void BasicImplicitStackAllocArrayCreationExpression()
        {
            this.RunTest("ImplicitStackAllocArrayCreationExpression", "BasicImplicitStackAllocArrayCreationExpression");
        }
    }
}
