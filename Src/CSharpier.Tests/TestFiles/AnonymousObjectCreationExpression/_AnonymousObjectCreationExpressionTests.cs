using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class AnonymousObjectCreationExpressionTests : BaseTest
    {
        [Test]
        public void AnonymousObjectCreationExpressions()
        {
            this.RunTest("AnonymousObjectCreationExpression", "AnonymousObjectCreationExpressions");
        }
    }
}
