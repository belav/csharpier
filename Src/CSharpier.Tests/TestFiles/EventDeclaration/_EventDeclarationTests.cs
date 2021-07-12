using NUnit.Framework;

namespace CSharpier.Tests.TestFiles.EventDeclaration
{
    public class EventDeclarationTests : BaseTest
    {
        [Test]
        public void EventDeclarations()
        {
            this.RunTest("EventDeclaration", "EventDeclarations");
        }
    }
}
