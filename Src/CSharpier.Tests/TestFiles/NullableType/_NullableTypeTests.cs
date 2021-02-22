using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
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
