using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.AliasQualifiedName
{
    public class AliasQualifiedNameTests : BaseTest
    {
        [Test]
        public void AliasQualifiedNames()
        {
            this.RunTest("AliasQualifiedName", "AliasQualifiedNames");
        }
    }
}
