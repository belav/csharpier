using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ObjectCreationExpressionTests : BaseTest
    {
        [Test]
        public void BasicObjectCreationExpression()
        {
            this.RunTest("ObjectCreationExpression", "BasicObjectCreationExpression");
        }
        [Test]
        public void ObjectCreationWithBiggerInitializer()
        {
            this.RunTest("ObjectCreationExpression", "ObjectCreationWithBiggerInitializer");
        }
        [Test]
        public void ObjectCreationWithInitializer()
        {
            this.RunTest("ObjectCreationExpression", "ObjectCreationWithInitializer");
        }
    }
}
