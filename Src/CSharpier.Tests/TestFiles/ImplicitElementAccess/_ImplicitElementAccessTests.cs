using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ImplicitElementAccessTests : BaseTest
    {
        [Test]
        public void ImplicitElementAccesses()
        {
            this.RunTest("ImplicitElementAccess", "ImplicitElementAccesses");
        }
    }
}
