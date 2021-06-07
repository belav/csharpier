using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ObnoxiousEdgeCaseTests : BaseTest
    {
        [Test]
        public void ObnoxiousEdgeCases()
        {
            this.RunTest("ObnoxiousEdgeCases", "ObnoxiousEdgeCases");
        }
    }
}
