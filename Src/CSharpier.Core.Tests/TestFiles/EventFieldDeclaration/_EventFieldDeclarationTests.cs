using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class EventFieldDeclarationTests : BaseTest
    {
        [Test]
        public void BasicEventFieldDeclaration()
        {
            this.RunTest("EventFieldDeclaration", "BasicEventFieldDeclaration");
        }
    }
}
