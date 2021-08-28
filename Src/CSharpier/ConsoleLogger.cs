using System;
using Microsoft.Extensions.Logging;

namespace CSharpier
{
    public class ConsoleLogger : ILogger
    {
        private static readonly object ConsoleLock = new();

        private readonly IConsole console;

        public ConsoleLogger(IConsole console)
        {
            this.console = console;
        }

        public virtual void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception, string> formatter
        ) {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            lock (ConsoleLock)
            {
                var message = formatter(state, exception!);

                this.console.ForegroundColor = GetColorLevel(logLevel);
                if (logLevel >= LogLevel.Warning)
                {
                    this.console.Write($"{logLevel} ");
                }
                this.console.WriteLine(message);
                this.console.ResetColor();

                if (exception == null)
                {
                    return;
                }
                this.console.WriteLine(exception.Message);
                this.console.WriteLine(exception.StackTrace);
                this.console.WriteLine();
            }
        }

        private ConsoleColor GetColorLevel(LogLevel logLevel) =>
            logLevel switch
            {
                LogLevel.Critical => ConsoleColor.DarkRed,
                LogLevel.Error => ConsoleColor.DarkRed,
                LogLevel.Warning => ConsoleColor.DarkYellow,
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Trace => ConsoleColor.Gray,
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
}
