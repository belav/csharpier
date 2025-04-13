namespace CSharpier.Validators;

internal class CSharpFormattingValidator(SyntaxNodeComparer syntaxNodeComparer)
    : IFormattingValidator
{
    public async Task<FormattingValidatorResult> ValidateAsync(CancellationToken cancellationToken)
    {
        var result = await syntaxNodeComparer.CompareSourceAsync(cancellationToken);
        if (!string.IsNullOrEmpty(result))
        {
            return new FormattingValidatorResult { Failed = true, FailureMessage = result };
        }

        return new FormattingValidatorResult();
    }
}
