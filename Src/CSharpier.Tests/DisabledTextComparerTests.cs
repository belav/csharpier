using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    [TestFixture]
    public class DisabledTextComparerTests
    {
        [Test]
        public void IsCodeBasicallyEqual_Should_Return_True_For_Basic_Case()
        {
            var before = "public string    Tester;";

            var after =
                @"public
string Tester;
"";

            using var stringReader = new StringReader(jsontext);";

            DisabledTextComparer.IsCodeBasicallyEqual(before, after).Should().BeTrue();
        }
    }
}
