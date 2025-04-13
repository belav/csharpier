using System.Text;
using System.Xml;
using Microsoft.XmlDiffPatch;

namespace CSharpier.Validators;

internal class XmlFormattingValidator(string originalXml, string formattedXml)
    : IFormattingValidator
{
    public Task<FormattingValidatorResult> ValidateAsync(CancellationToken cancellationToken)
    {
        var xmlDiff = new XmlDiff(XmlDiffOptions.IgnoreWhitespace);
        using var originalReader = XmlReader.Create(new StringReader(originalXml));
        using var formattedReader = XmlReader.Create(new StringReader(formattedXml));
        var stringBuilder = new StringBuilder();
        using var diffWriter = XmlWriter.Create(stringBuilder);

        var result = new FormattingValidatorResult();

        if (!xmlDiff.Compare(originalReader, formattedReader, diffWriter))
        {
            result.Failed = true;
        }

        return Task.FromResult(result);
    }
}
