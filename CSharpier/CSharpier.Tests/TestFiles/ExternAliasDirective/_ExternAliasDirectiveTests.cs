using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ExternAliasDirectiveTests : BaseTest
    {
        [Test]
        public void BasicExternAliasDirective()
        {
            this.RunTest("ExternAliasDirective", "BasicExternAliasDirective");
        }
        [Test]
        public void ExternAliasComments()
        {
            this.RunTest("ExternAliasDirective", "ExternAliasComments");
        }
    }
}
