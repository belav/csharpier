using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class NullableTypeTests : BaseTest
    {
        [Test]
        public void BasicNullableType()
        {
            this.RunTest("NullableType", "BasicNullableType");
        }
    }
}
