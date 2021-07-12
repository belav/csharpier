using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.EventFieldDeclaration
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
