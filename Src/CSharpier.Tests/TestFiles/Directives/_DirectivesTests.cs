using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class DirectivesTests : BaseTest
    {
        [Test]
        public void Directives()
        {
            this.RunTest("Directives", "Directives");
        }
        [Test]
        public void Regions()
        {
            this.RunTest("Directives", "Regions");
        }
    }
}
