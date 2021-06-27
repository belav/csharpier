using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class FileEncodingTests : BaseTest
    {
        [Test]
        public void Ansi()
        {
            this.RunTest("FileEncoding", "Ansi");
        }

        [Test]
        [Ignore(
            "This fails on github actions because of line encodings. But if they are changed it fails on windows due to line endings. See #115"
        )]
        public void USC2LEBOM()
        {
            this.RunTest("FileEncoding", "USC2LEBOM");
        }
        [Test]
        public void UTF8()
        {
            this.RunTest("FileEncoding", "UTF8");
        }
        [Test]
        public void UTF8BOM()
        {
            this.RunTest("FileEncoding", "UTF8BOM");
        }
    }
}
