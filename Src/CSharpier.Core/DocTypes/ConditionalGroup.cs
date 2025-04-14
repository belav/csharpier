namespace CSharpier.Core.DocTypes;

internal sealed class ConditionalGroup : Group
{
    public ConditionalGroup(Doc[] options)
    {
        if (options.Length == 0)
        {
            throw new ArgumentException("Options was an empty array");
        }

        this.Options = options;
        this.Contents = options[0];
    }

    public Doc[] Options { get; }
}
