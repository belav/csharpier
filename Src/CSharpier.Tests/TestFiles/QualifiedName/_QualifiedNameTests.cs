using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class QualifiedNameTests : BaseTest
    {
        [Test]
        public void UsingWithDots()
        {
            this.RunTest("QualifiedName", "UsingWithDots");
        }
    }
}
