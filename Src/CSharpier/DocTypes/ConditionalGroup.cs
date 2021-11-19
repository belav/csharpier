namespace CSharpier.DocTypes;

internal class ConditionalGroup : Group
{
    public ConditionalGroup(Doc[] options)
    {
        if (options.Length == 0)
        {
            throw new ArgumentException("Options was an empty array");
        }
        Options = options;
        Contents = options[0];
    }

    public Doc[] Options { get; }
}
