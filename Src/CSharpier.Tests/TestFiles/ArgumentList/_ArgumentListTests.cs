using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class ArgumentListTests : BaseTest
    {
        [Test]
        public void BasicArgumentList()
        {
            this.RunTest("ArgumentList", "BasicArgumentList");
        }
        [Test]
        public void EmptyArgumentListStaysOnSameLine()
        {
            this.RunTest("ArgumentList", "EmptyArgumentListStaysOnSameLine");
        }
        [Test]
        public void LongArgumentList()
        {
            this.RunTest("ArgumentList", "LongArgumentList");
        }
        [Test]
        public void NamedArguments()
        {
            this.RunTest("ArgumentList", "NamedArguments");
        }
    }
}
