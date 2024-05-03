using System.Security.Cryptography;
using System.Text;

namespace CSharpier.Utilities;

internal static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string value, string otherValue)
    {
        return string.Compare(value, otherValue, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool ContainsIgnoreCase(this string value, string otherValue)
    {
        return value.ToLower().Contains(otherValue.ToLower());
    }

    public static bool StartsWithIgnoreCase(this string value, string otherValue)
    {
        return value.StartsWith(otherValue, StringComparison.OrdinalIgnoreCase);
    }

    public static bool EndsWithIgnoreCase(this string value, string otherValue)
    {
        return value.EndsWith(otherValue, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsBlank(this string? value)
    {
        return value == null || string.IsNullOrEmpty(value.Trim());
    }

    // some unicode characters should be considered size of 2 when calculating how big this string will be when printed
    public static int GetPrintedWidth(this string value)
    {
        return value.Sum(CharacterSizeCalculator.CalculateWidth);
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
