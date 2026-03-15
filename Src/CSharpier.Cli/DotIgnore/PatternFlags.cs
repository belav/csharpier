namespace CSharpier.Cli.DotIgnore;

[Flags]
#pragma warning disable CA1711
internal enum PatternFlags
#pragma warning restore CA1711
{
    NONE = 0,
    NEGATION = 1,
    ABSOLUTE_PATH = 2,
    DIRECTORY = 4,
}
