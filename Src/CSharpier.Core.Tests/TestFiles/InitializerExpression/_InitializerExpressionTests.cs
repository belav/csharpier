using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class InitializerExpressionTests : BaseTest
    {
        [Test]
        public void BasicArrayInitializerExpression()
        {
            this.RunTest("InitializerExpression", "BasicArrayInitializerExpression");
        }
        [Test]
        public void BasicCollectionInitializerExpression()
        {
            this.RunTest("InitializerExpression", "BasicCollectionInitializerExpression");
        }
        [Test]
        public void BasicComplexElementInitializerExpression()
        {
            this.RunTest("InitializerExpression", "BasicComplexElementInitializerExpression");
        }
        [Test]
        public void LongItemsArrayInitializer()
        {
            this.RunTest("InitializerExpression", "LongItemsArrayInitializer");
        }
    }
}
