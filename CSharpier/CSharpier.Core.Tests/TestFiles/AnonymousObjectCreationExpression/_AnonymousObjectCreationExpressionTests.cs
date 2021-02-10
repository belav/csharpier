using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class AnonymousObjectCreationExpressionTests : BaseTest
    {
        [Test]
        public void BasicAnonymousObjectCreationExpression()
        {
            this.RunTest("AnonymousObjectCreationExpression", "BasicAnonymousObjectCreationExpression");
        }
        [Test]
        public void MultipleProperties()
        {
            this.RunTest("AnonymousObjectCreationExpression", "MultipleProperties");
        }
        [Test]
        public void NoNames()
        {
            this.RunTest("AnonymousObjectCreationExpression", "NoNames");
        }
    }
}
