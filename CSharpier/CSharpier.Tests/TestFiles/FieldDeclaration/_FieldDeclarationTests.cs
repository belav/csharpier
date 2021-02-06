using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class FieldDeclarationTests : BaseTest
    {
        [Test]
        public void BasicField()
        {
            this.RunTest("FieldDeclaration", "BasicField");
        }
        [Ignore("TODO comments")]
        [Test]
        public void FieldDeclarationComments()
        {
            this.RunTest("FieldDeclaration", "FieldDeclarationComments");
        }
        [Test]
        public void FixedFieldWithSize()
        {
            this.RunTest("FieldDeclaration", "FixedFieldWithSize");
        }
        [Test]
        public void NamespacedField()
        {
            this.RunTest("FieldDeclaration", "NamespacedField");
        }
    }
}
