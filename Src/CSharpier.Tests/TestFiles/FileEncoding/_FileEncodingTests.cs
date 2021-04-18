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
