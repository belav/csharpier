using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class StackAllocArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void BasicStackAllocArrayCreationExpression()
        {
            this.RunTest("StackAllocArrayCreationExpression", "BasicStackAllocArrayCreationExpression");
        }
    }
}
