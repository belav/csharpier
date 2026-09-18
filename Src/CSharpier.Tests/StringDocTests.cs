using AwesomeAssertions;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Tests;

public class StringDocTests
{
    [Test]
    [Arguments("", 0)]
    [Arguments("someValue", 9)]
    [Arguments("가", 2)]
    [Arguments("가가가", 6)]
    [Arguments("var x = \"가가\";", 15)]
    public void PrintedWidth_Should_Count_Wide_Characters_As_Two_Columns(string value, int expected)
    {
        var stringDoc = new StringDoc(value);

        stringDoc.PrintedWidth.Should().Be(expected);
        stringDoc.PrintedWidth.Should().Be(value.GetPrintedWidth());
    }
}
