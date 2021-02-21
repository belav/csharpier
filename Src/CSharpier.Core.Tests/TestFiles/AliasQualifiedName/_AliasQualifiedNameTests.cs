using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class AliasQualifiedNameTests : BaseTest
    {
        [Test]
        public void BasicAliasQualifiedName()
        {
            this.RunTest("AliasQualifiedName", "BasicAliasQualifiedName");
        }
    }
}
