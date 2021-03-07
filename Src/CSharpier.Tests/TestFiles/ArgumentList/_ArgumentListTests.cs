using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ArgumentListTests : BaseTest
    {
        [Test]
        public void ArgumentLists()
        {
            this.RunTest("ArgumentList", "ArgumentLists");
        }
    }
}
