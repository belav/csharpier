using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AttributeList
{
    public class AttributeListTests : BaseTest
    {
        [Test]
        public void AttributeListNewLines()
        {
            this.RunTest("AttributeList", "AttributeListNewLines");
        }
        [Test]
        public void AttributeLists()
        {
            this.RunTest("AttributeList", "AttributeLists");
        }
    }
}
