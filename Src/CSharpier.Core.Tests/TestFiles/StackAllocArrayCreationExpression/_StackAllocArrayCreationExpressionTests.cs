using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
