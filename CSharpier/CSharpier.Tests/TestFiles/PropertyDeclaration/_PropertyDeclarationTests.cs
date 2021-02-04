using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class PropertyDeclarationTests : BaseTest
    {
        [Test]
        public void BasicProperty()
        {
            this.RunTest("PropertyDeclaration", "BasicProperty");
        }
        [Test]
        public void LineBreaks()
        {
            this.RunTest("PropertyDeclaration", "LineBreaks");
        }
        [Test]
        public void PropertyModifiers()
        {
            this.RunTest("PropertyDeclaration", "PropertyModifiers");
        }
        [Test]
        public void PropertyWithBackingValue()
        {
            this.RunTest("PropertyDeclaration", "PropertyWithBackingValue");
        }
        [Test]
        public void PropertyWithInitializer()
        {
            this.RunTest("PropertyDeclaration", "PropertyWithInitializer");
        }
        [Test]
        public void PropertyWithLambdaAccessors()
        {
            this.RunTest("PropertyDeclaration", "PropertyWithLambdaAccessors");
        }
        [Test]
        public void PropertyWithLambdaBody()
        {
            this.RunTest("PropertyDeclaration", "PropertyWithLambdaBody");
        }
        [Test]
        public void PropertyWithThisExpression()
        {
            this.RunTest("PropertyDeclaration", "PropertyWithThisExpression");
        }
    }
}
