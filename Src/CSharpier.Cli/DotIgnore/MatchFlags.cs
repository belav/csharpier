namespace CSharpier.Cli.DotIgnore;

// TODO #1768 kill this? everything is PATHNAME
[Flags]
#pragma warning disable CA1711
internal enum MatchFlags
#pragma warning restore CA1711
{
    NONE = 0,
    CASEFOLD = 1,
    PATHNAME = 2,
}
