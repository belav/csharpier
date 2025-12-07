namespace CSharpier.Cli.DotIgnore;

[Flags]
#pragma warning disable CA1711
internal enum MatchFlags
#pragma warning restore CA1711
{
    NONE = 0,
    CASEFOLD = 1,
    PATHNAME = 2,
}
