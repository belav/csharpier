using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class CommentsTests : BaseTest
    {
        [Test]
        public void ClassComments()
        {
            this.RunTest("Comments", "ClassComments");
        }
        [Test]
        public void LeadingAndTrailingComments()
        {
            this.RunTest("Comments", "LeadingAndTrailingComments");
        }
        [Test]
        public void MethodComments()
        {
            this.RunTest("Comments", "MethodComments");
        }
    }
}
