using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class EventFieldDeclarationTests : BaseTest
    {
        [Test]
        public void EventFieldDeclarations()
        {
            this.RunTest("EventFieldDeclaration", "EventFieldDeclarations");
        }
    }
}
