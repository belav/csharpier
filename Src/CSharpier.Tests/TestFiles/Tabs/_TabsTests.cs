using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
