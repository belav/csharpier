using System.Buffers;
using CSharpier.Core.CSharp.SyntaxPrinter;

namespace CSharpier.Core.CSharp;

internal static class CSharpScanner
{
    private const string IfPreprocessor = "#if";
    private const string CSharpierIgnore = "// csharpier-ignore";

#if NET9_0_OR_GREATER
    private static readonly SearchValues<string> KeyValues = SearchValues.Create(
        [IfPreprocessor, CSharpierIgnore],
        StringComparison.Ordinal
    );

    public static PrintingContext.CodeInformation Scan(string code)
    {
        var index = code.AsSpan().IndexOfAny(KeyValues);
        if (index < 0)
        {
            return new PrintingContext.CodeInformation(false, false);
        }

        var slice = code.AsSpan(index);
        if (slice.StartsWith(IfPreprocessor))
        {
            return new PrintingContext.CodeInformation(
                true,
                slice.Contains(CSharpierIgnore.AsSpan(), StringComparison.Ordinal)
            );
        }

        return new PrintingContext.CodeInformation(
            slice.Contains(IfPreprocessor.AsSpan(), StringComparison.Ordinal),
            true
        );
    }
#else
    public static PrintingContext.CodeInformation Scan(string code)
    {
        var hasPreprocessor = code.Contains(IfPreprocessor);
        var hasCSharpierIgnore = code.Contains(CSharpierIgnore);
        return new PrintingContext.CodeInformation(hasPreprocessor, hasCSharpierIgnore);
    }
#endif
}
