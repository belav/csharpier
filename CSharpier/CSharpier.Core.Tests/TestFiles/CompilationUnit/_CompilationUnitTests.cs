using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class CompilationUnitTests : BaseTest
    {
        [Test]
        public void MultipleClasses()
        {
            this.RunTest("CompilationUnit", "MultipleClasses");
        }
    }
}
