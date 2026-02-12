namespace CSharpier.Cli;

internal enum LogFormat
{
    /// <summary>
    /// Formats messages in a human readable way for console interaction.
    /// </summary>
    Console,

    /// <summary>
    /// Formats messages in standard error/warning format for MSBuild.
    /// </summary>
    MsBuild,

    /// <summary>
    /// Emits a SARIF 2.1.0 log document containing warnings and errors.
    /// </summary>
    Sarif,
}
