using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class TupleTypeTests : BaseTest
    {
        [Test]
        public void BasicTupleType()
        {
            this.RunTest("TupleType", "BasicTupleType");
        }
    }
}
