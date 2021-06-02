using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class RefTypeTests : BaseTest
    {
        [Test]
        public void RefTypes()
        {
            this.RunTest("RefType", "RefTypes");
        }
    }
}
