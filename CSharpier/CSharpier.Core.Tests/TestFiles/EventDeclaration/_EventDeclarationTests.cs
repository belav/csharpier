using CSharpier.Core.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Core.Tests.TestFiles
{
    public class EventDeclarationTests : BaseTest
    {
        [Test]
        public void BasicEventDeclaration()
        {
            this.RunTest("EventDeclaration", "BasicEventDeclaration");
        }
    }
}
