using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class UsingDirectiveTests : BaseTest
    {
        [Test]
        public void UsingDirectives()
        {
            this.RunTest("UsingDirective", "UsingDirectives");
        }
    }
}
