using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ImplicitArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void BasicImplicitArrayCreationExpression()
        {
            this.RunTest(
                "ImplicitArrayCreationExpression",
                "BasicImplicitArrayCreationExpression");
        }
        [Test]
        public void ImplicityArrayWithCommas()
        {
            this.RunTest(
                "ImplicitArrayCreationExpression",
                "ImplicityArrayWithCommas");
        }
    }
}
