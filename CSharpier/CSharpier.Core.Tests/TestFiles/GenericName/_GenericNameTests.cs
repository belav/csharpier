using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class GenericNameTests : BaseTest
    {
        [Test]
        public void BasicGenericName()
        {
            this.RunTest("GenericName", "BasicGenericName");
        }
    }
}
