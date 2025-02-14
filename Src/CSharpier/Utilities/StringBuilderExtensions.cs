using System.Globalization;
using System.Text;

namespace CSharpier.Utilities;

internal static class StringBuilderExtensions
{
#if NETSTANDARD2_0
    public static void AppendLine(
        this StringBuilder value,
        CultureInfo cultureInfo,
        string otherValue
    )
    {
        value.AppendLine(otherValue);
    }

    public static void Append(this StringBuilder value, CultureInfo cultureInfo, string otherValue)
    {
        value.Append(otherValue);
    }
#endif

    public static bool EndsWithNewLineAndWhitespace(this StringBuilder stringBuilder)
    {
        for (var index = 1; index <= stringBuilder.Length; index++)
        {
            var next = stringBuilder[^index];
            if (next is ' ' or '\t')
            {
                continue;
            }
            else if (next == '\n')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public static int TrimTrailingWhitespace(this StringBuilder stringBuilder)
    {
        if (stringBuilder.Length == 0)
        {
            return 0;
        }

        var trimmed = 0;
        for (; trimmed < stringBuilder.Length; trimmed++)
        {
            if (stringBuilder[^(trimmed + 1)] is not ' ' and not '\t')
            {
                break;
            }
        }

        stringBuilder.Length -= trimmed;
        return trimmed;
    }
}
