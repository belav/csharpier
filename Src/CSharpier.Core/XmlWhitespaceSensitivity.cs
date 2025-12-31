namespace CSharpier.Core;

internal enum XmlWhitespaceSensitivity
{
    Strict,

    // TODO #1794 figure out what this actually means code wise, should it be named something besides xaml?
    Xaml,

    // TODO #1794 do a lot more testing with this, can review the repos once the file extension stuff is figured out.
    Ignore,
}
