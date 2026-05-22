using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.CSharp;

namespace CSharpier.Tests;

internal sealed class AllowFieldAttributeOnSameLineTests
{
    private static PrinterOptions CreateOptions(bool allowFieldAttributeOnSameLine)
    {
        return new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Strict)
        {
            Width = 100,
            FormattingStyle = allowFieldAttributeOnSameLine
                ? FormattingStyle.Resharper
                : FormattingStyle.Default,
        };
    }

    [Test]
    public async Task Field_With_Single_Attribute_Stays_On_Same_Line_When_Enabled()
    {
        var code = "[SerializeField] private Button _buttonQuit;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("[SerializeField] private Button _buttonQuit;");
    }

    [Test]
    public async Task Field_With_Single_Attribute_Breaks_Line_When_Disabled()
    {
        var code = "[SerializeField] private Button _buttonQuit;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: false)
        );
        result.Code.Should().Contain("[SerializeField]\n    private Button _buttonQuit;");
    }

    [Test]
    public async Task Field_With_Long_Attribute_Breaks_When_Exceeds_Width()
    {
        var code =
            "[VeryLongAttributeName(SomeFlag.SomeValue, SomeOtherFlag.SomeOtherLongValue)] private Button _buttonQuit;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("]\n    private Button _buttonQuit;");
    }

    [Test]
    public async Task Field_With_Two_Attributes_Stays_On_Same_Line_When_Enabled()
    {
        var code = "[SerializeField] [HideInInspector] private Button _buttonQuit;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("[SerializeField] [HideInInspector] private Button _buttonQuit;");
    }

    [Test]
    public async Task Field_With_Two_Attributes_On_Separate_Lines_Preserves_Breaks_When_Enabled()
    {
        var code = "[Header(\"Title\")]\n    [SerializeField] private Button _buttonQuit;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("[Header(\"Title\")]\n    [SerializeField] private Button _buttonQuit;");
    }

    [Test]
    public async Task Field_With_Three_Attributes_Groups_Attrs_Then_Breaks_Field_When_Enabled()
    {
        var code = "[SerializeField] [HideInInspector] [Header(\"Title\")] private Text _labelTitle;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("[SerializeField] [HideInInspector] [Header(\"Title\")]\n    private Text _labelTitle;");
    }

    [Test]
    public async Task Field_With_More_Than_Three_Attributes_Breaks_Lines_When_Enabled()
    {
        var code =
            "[SerializeField]\n    [HideInInspector]\n    [Header(\"Title\")]\n    [Tooltip(\"tip\")]\n    private Text _labelTitle;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("[SerializeField]\n    [HideInInspector]\n    [Header(\"Title\")]\n    [Tooltip(\"tip\")]\n    private Text _labelTitle;");
    }

    [Test]
    public async Task Field_With_Three_Comma_Separated_Attrs_Breaks_Field_When_Enabled()
    {
        var code = "[Obsolete, NonSerialized, CLSCompliant(true)] private int _field;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("]\n    private int _field;");
    }

    [Test]
    public async Task Field_With_Mixed_Attrs_Totaling_Three_Breaks_Field_When_Enabled()
    {
        var code = "[Obsolete, NonSerialized]\n    [Header(\"X\")]\n    private int _field;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("]\n    private int _field;");
    }

    [Test]
    public async Task Field_With_Four_Comma_Separated_Attrs_Breaks_All_When_Enabled()
    {
        var code =
            "[Obsolete, NonSerialized, CLSCompliant(true), Browsable(false)] private int _field;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("]\n    private int _field;");
    }

    [Test]
    public async Task Field_With_Two_Comma_Separated_Attrs_Stays_On_Same_Line_When_Enabled()
    {
        var code = "[Obsolete, NonSerialized] private int _field;\n";
        var result = await CSharpFormatter.FormatAsync(
            WrapInClass(code),
            CreateOptions(allowFieldAttributeOnSameLine: true)
        );
        result.Code.Should().Contain("[Obsolete, NonSerialized] private int _field;");
    }

    private static string WrapInClass(string fieldCode)
    {
        return $"class TestClass\n{{\n    {fieldCode}}}\n";
    }
}
