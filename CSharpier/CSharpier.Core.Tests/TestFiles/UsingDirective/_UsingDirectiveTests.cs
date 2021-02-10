using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class UsingDirectiveTests : BaseTest
    {
        [Test]
        public void BasicUsingDirective()
        {
            this.RunTest("UsingDirective", "BasicUsingDirective");
        }
        [Test]
        public void UsingDirectiveComments()
        {
            this.RunTest("UsingDirective", "UsingDirectiveComments");
        }
        [Test]
        public void UsingWithAlias()
        {
            this.RunTest("UsingDirective", "UsingWithAlias");
        }
        [Test]
        public void UsingWithStatic()
        {
            this.RunTest("UsingDirective", "UsingWithStatic");
        }
    }
}
