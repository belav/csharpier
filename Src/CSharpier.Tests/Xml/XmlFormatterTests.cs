using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.Xml;

namespace CSharpier.Tests.Xml;

internal sealed class XmlFormatterTests
{
    private const string Xml =
        "<Project><PropertyGroup><Nullable>enable</Nullable></PropertyGroup></Project>";

    [Test]
    public async Task Format_Should_Not_Include_AST_When_Not_Requested()
    {
        var result = await XmlFormatter.FormatAsync(Xml, OptionsWith(includeAst: false));

        result.AST.Should().BeEmpty();
    }

    [Test]
    public async Task Format_Should_Include_AST_When_Requested()
    {
        var result = await XmlFormatter.FormatAsync(Xml, OptionsWith(includeAst: true));

        result.AST.Should().NotBeEmpty();
    }

    private static PrinterOptions OptionsWith(bool includeAst)
    {
        return new PrinterOptions(Formatter.XML, XmlWhitespaceSensitivity.Strict)
        {
            IncludeAST = includeAst,
        };
    }
}
