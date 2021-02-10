using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class ArrayTypeTests : BaseTest
    {
        [Test]
        public void BasicArrayType()
        {
            this.RunTest("ArrayType", "BasicArrayType");
        }
    }
}
