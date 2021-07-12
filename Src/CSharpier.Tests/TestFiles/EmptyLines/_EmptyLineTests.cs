using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.EmptyLines
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
