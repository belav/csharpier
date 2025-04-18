using System.Text;
using System.Xml;
using CSharpier.Core.Validators;
using Microsoft.XmlDiffPatch;

namespace CSharpier.Core.Xml;

internal class XmlFormattingValidator(string originalXml, string formattedXml)
    : IFormattingValidator
{
    public Task<FormattingValidatorResult> ValidateAsync(CancellationToken cancellationToken)
    {
        var xmlDiff = new XmlDiff(XmlDiffOptions.IgnoreWhitespace);
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse
        };
        using var originalReader = XmlReader.Create(new StringReader(originalXml), settings);
        using var formattedReader = XmlReader.Create(new StringReader(formattedXml), settings);
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
