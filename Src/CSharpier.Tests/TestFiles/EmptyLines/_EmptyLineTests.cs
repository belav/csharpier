using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class EmptyLineTests : BaseTest
    {
        [Test]
        public void EmptyLines()
        {
            this.RunTest("EmptyLines", "EmptyLines");
        }
    }
}
