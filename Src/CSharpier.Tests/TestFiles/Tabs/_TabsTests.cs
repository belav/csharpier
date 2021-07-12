using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.Tabs
{
    public class TabsTests : BaseTest
    {
        [Test]
        public void Tabs()
        {
            this.RunTest("Tabs", "Tabs", useTabs: true);
        }
    }
}
