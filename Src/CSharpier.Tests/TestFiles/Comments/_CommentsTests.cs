using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.Comments
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
