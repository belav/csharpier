using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.UsingDirective
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
