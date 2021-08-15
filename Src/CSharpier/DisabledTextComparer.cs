using System.Text;

namespace CSharpier
{
    public class DisabledTextComparer
    {
        public static bool IsCodeBasicallyEqual(string code, string formattedCode)
        {
            var squashCode = Squash(code);
            var squashFormattedCode = Squash(formattedCode);

            var result = squashCode == squashFormattedCode;
            return result;
        }

        private static string Squash(string code)
        {
            var result = new StringBuilder();
            for (var index = 0; index < code.Length; index++)
            {
                var nextChar = code[index];
                if (nextChar is ' ' or '\t' or '\r' or '\n')
                {
                    if (
                        index != code.Length - 1
                        && (
                            result.Length == 0
                            || result[^1]
                                is not (' '
                                    or '('
                                    or ')'
                                    or '{'
                                    or '}'
                                    or '['
                                    or ']'
                                    or '<'
                                    or '>'
                                    or ','
                                    or ':'
                                    or ';')
                        )
                    ) {
                        result.Append(' ');
                    }
                }
                else if (
                    nextChar
                        is '('
                            or ')'
                            or '{'
                            or '}'
                            or ']'
                            or '['
                            or '.'
                            or '<'
                            or '>'
                            or ';'
                            or ':'
                    && result.Length > 0
                    && result[^1] is ' '
                ) {
                    result[^1] = nextChar;
                }
                else
                {
                    result.Append(nextChar);
                }
            }

            return result.ToString();
        }
    }
}
