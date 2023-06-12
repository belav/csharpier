using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

public class ConsoleLogger : ILogger
{
    private static readonly object ConsoleLock = new();

    private readonly IConsole console;
    private readonly LogLevel loggingLevel;

    public ConsoleLogger(IConsole console, LogLevel loggingLevel)
    {
        this.console = console;
        this.loggingLevel = loggingLevel;
    }

    public virtual void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception, string> formatter
    )
    {
        if (logLevel < this.loggingLevel)
        {
            return;
        }

        void Write(string value)
        {
            if (logLevel >= LogLevel.Error)
            {
                this.console.WriteError(value);
            }
            else
            {
                this.console.Write(value);
            }
        }

        void WriteLine(string? value = null)
        {
            if (logLevel >= LogLevel.Error)
            {
                this.console.WriteErrorLine(value);
            }
            else
            {
                this.console.WriteLine(value);
            }
        }

        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        lock (ConsoleLock)
        {
            var message = formatter(state, exception!);

            if (logLevel >= LogLevel.Warning)
            {
                this.console.ForegroundColor = GetColorLevel(logLevel);
                Write($"{logLevel} ");
                this.console.ResetColor();
            }

            var stringReader = new StringReader(message);
            var line = stringReader.ReadLine();
            WriteLine(line);
            while ((line = stringReader.ReadLine()) != null)
            {
                WriteLine("  " + line);
            }

            if (exception == null)
            {
                return;
            }

            WriteLine("  " + exception.Message);
            if (exception.StackTrace != null)
            {
                stringReader = new StringReader(exception.StackTrace);
                while ((line = stringReader.ReadLine()) != null)
                {
                    WriteLine("  " + line);
                }
            }

            WriteLine();
        }
    }

    private static ConsoleColor GetColorLevel(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Critical => ConsoleColor.DarkRed,
            LogLevel.Error => ConsoleColor.DarkRed,
            LogLevel.Warning => ConsoleColor.DarkYellow,
            _ => ConsoleColor.White,
        };

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}
