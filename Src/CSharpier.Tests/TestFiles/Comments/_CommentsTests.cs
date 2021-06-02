using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class CommentsTests : BaseTest
    {
        [Test]
        public void Comments()
        {
            this.RunTest("Comments", "Comments");
        }
    }
}
