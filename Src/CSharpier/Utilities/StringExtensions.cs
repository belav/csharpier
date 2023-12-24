using System.Security.Cryptography;
using System.Text;

namespace CSharpier.Utilities;

internal static class StringExtensions
{
    public static string CalculateHash(this string value)
    {
        using var hasher = MD5.Create();
        var hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
        return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();
    }

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

    // this will eventually deal with the visual width not being the same as the code width https://github.com/belav/csharpier/issues/260
    public static int GetPrintedWidth(this string value)
    {
        return value.Length;
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
