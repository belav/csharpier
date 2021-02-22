using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class AttributeListTests : BaseTest
    {
        [Test]
        public void AttributeListComments()
        {
            this.RunTest("AttributeList", "AttributeListComments");
        }
        [Test]
        public void AttributeListNewLines()
        {
            this.RunTest("AttributeList", "AttributeListNewLines");
        }
        [Test]
        public void AttributeNameColon()
        {
            this.RunTest("AttributeList", "AttributeNameColon");
        }
        [Test]
        public void AttributeNameEquals()
        {
            this.RunTest("AttributeList", "AttributeNameEquals");
        }
        [Test]
        public void AttributeParameters()
        {
            this.RunTest("AttributeList", "AttributeParameters");
        }
        [Test]
        public void AttributeTargetSpecifier()
        {
            this.RunTest("AttributeList", "AttributeTargetSpecifier");
        }
        [Test]
        public void BasicAttributeList()
        {
            this.RunTest("AttributeList", "BasicAttributeList");
        }
    }
}
