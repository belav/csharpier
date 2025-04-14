namespace CSharpier.Core.Utilities;

internal static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string value, string otherValue)
    {
        return string.Equals(value, otherValue, StringComparison.OrdinalIgnoreCase);
    }

    public static bool StartsWithIgnoreCase(this string value, string otherValue)
    {
        return value.StartsWith(otherValue, StringComparison.OrdinalIgnoreCase);
    }

    public static bool EndsWithIgnoreCase(this string value, string otherValue)
    {
        return value.EndsWith(otherValue, StringComparison.OrdinalIgnoreCase);
    }

#if NETSTANDARD2_0
    public static bool StartsWith(this string value, char otherValue)
    {
        return value.StartsWith(otherValue.ToString());
    }

    public static bool EndsWith(this string value, char otherValue)
    {
        return value.EndsWith(otherValue.ToString());
    }

    public static int IndexOf(this string value, char otherValue)
    {
        return value.IndexOf(otherValue.ToString(), StringComparison.Ordinal);
    }
#endif

    // some unicode characters should be considered size of 2 when calculating how big this string will be when printed
    public static int GetPrintedWidth(this string value)
    {
        var sum = 0;
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var character in value)
        {
            sum += CharacterSizeCalculator.CalculateWidth(character);
        }

        return sum;
    }

    public static int CalculateCurrentLeadingIndentation(this string line, int indentSize)
    {
        var result = 0;
        foreach (var character in line)
        {
            if (character == ' ')
            {
                result += 1;
            }
            // I'm not sure why this converts tabs to the size of an indent
            // I'd think this should be based on if UseTabs is true or not
            // if using tabs, this should be considered one
            // but then how do we convert spaces to tabs?
            // this seems to work, and it came from the comments code
            else if (character == '\t')
            {
                result += indentSize;
            }
            else
            {
                break;
            }
        }

        return result;
    }
}
