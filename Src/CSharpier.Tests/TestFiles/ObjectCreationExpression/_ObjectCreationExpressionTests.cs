using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ObjectCreationExpressionTests : BaseTest
    {
        [Test]
        public void ObjectCreationExpressions()
        {
            this.RunTest("ObjectCreationExpression", "ObjectCreationExpressions");
        }
    }
}
