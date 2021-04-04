using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ExtraLinesTests : BaseTest
    {
        [Test]
        public void ExtraLines()
        {
            this.RunTest("ExtraLines", "ExtraLines");
        }
    }
}
