using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
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
