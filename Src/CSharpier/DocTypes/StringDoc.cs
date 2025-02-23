namespace CSharpier.DocTypes;

internal sealed class StringDoc(string value, bool isDirective = false) : Doc
{
    public string Value { get; } = value;
    public bool IsDirective { get; } = isDirective;

    public static StringDoc Create(string value)
    {
        if (value.Length != 1)
        {
            return new StringDoc(value);
        }

        return value switch
        {
            " " => SpaceString,
            "\t" => TabString,
            "," => CommaString,
            "=" => EqualsString,
            "." => DotString,
            "{" => OpenBraceString,
            "}" => ClosedBraceString,
            "(" => OpenBracketString,
            ")" => ClosedBracketString,
            ";" => SemiColonString,

            _ => new StringDoc(value),
        };
    }

    private static readonly StringDoc SpaceString = new(" ");
    private static readonly StringDoc TabString = new("\t");
    private static readonly StringDoc CommaString = new(",");
    private static readonly StringDoc EqualsString = new("=");
    private static readonly StringDoc DotString = new(".");
    private static readonly StringDoc OpenBraceString = new("{");
    private static readonly StringDoc ClosedBraceString = new("}");
    private static readonly StringDoc OpenBracketString = new("(");
    private static readonly StringDoc ClosedBracketString = new(")");
    private static readonly StringDoc SemiColonString = new(";");
}
