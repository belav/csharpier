using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.ExternAliasDirective
{
    public class ExternAliasDirectiveTests : BaseTest
    {
        [Test]
        public void ExternAliasDirectives()
        {
            this.RunTest("ExternAliasDirective", "ExternAliasDirectives");
        }
    }
}
