using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class RefTypeTests : BaseTest
    {
        [Test]
        public void BasicRefType()
        {
            this.RunTest("RefType", "BasicRefType");
        }
    }
}
