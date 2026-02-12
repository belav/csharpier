using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class ConsoleLogger(IConsole console, LogLevel loggingLevel) : ILogger
{
    private static readonly object ConsoleLock = new();

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception, string> formatter
    )
    {
        if (logLevel < loggingLevel)
        {
            return;
        }

        if (!IsEnabled(logLevel))
        {
            return;
        }

        lock (ConsoleLock)
        {
            var (path, region, _, message) = LoggerExtensions.ExtractState(state);

            var messageToLog = message ?? formatter(state, exception!);

            if (logLevel >= LogLevel.Warning)
            {
                console.ForegroundColor = GetColorLevel(logLevel);
                console.Write($"{logLevel.ToString().ToUpper()}: ");
                console.ResetColor();
            }

            var regionString =
                region == null
                    ? ""
                    : $"{region.StartLine}:{region.StartCharacter} ";
            
            console.WriteLine($"{path}{regionString}{messageToLog}");

            if (exception != null)
            {
                console.WriteLine(exception.ToString());
            }
        }
    }

    private static ConsoleColor GetColorLevel(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Critical => ConsoleColor.DarkRed,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Warning => ConsoleColor.Yellow,
            _ => ConsoleColor.White
        };

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull =>
        throw new NotImplementedException();
}
