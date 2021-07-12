using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ImplicitElementAccess
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
