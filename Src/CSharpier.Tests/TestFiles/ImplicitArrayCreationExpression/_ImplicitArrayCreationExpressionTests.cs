using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ImplicitArrayCreationExpressionTests : BaseTest
    {
        [Test]
        public void ImplicitArrayCreationExpressions()
        {
            this.RunTest("ImplicitArrayCreationExpression", "ImplicitArrayCreationExpressions");
        }
    }
}
