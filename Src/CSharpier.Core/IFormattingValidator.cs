namespace CSharpier.Core.Validators;

internal interface IFormattingValidator
{
    Task<FormattingValidatorResult> ValidateAsync(CancellationToken cancellationToken);
}

internal class FormattingValidatorResult
{
    public bool Failed { get; set; }
    public string? FailureMessage { get; set; }
}
