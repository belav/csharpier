using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ImplicitElementAccessTests : BaseTest
    {
        [Test]
        public void BasicImplicitElementAccess()
        {
            this.RunTest("ImplicitElementAccess", "BasicImplicitElementAccess");
        }
    }
}
