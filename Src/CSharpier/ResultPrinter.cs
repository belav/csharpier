using Microsoft.Extensions.Logging;

namespace CSharpier
{
    public static class ResultPrinter
    {
        public static void PrintResults(
            CommandLineFormatterResult result,
            ILogger logger,
            CommandLineOptions commandLineOptions
        )
        {
            logger.LogInformation(
                PadToSize("Total time: ", 80) + ReversePad(result.ElapsedMilliseconds + "ms")
            );
            PrintResultLine("Total files", result.Files, logger);

            if (!commandLineOptions.Fast)
            {
                if (result.FailedSyntaxTreeValidation != 0)
                {
                    PrintResultLine(
                        "Failed syntax tree validation",
                        result.FailedSyntaxTreeValidation,
                        logger
                    );
                }

                if (result.ExceptionsFormatting != 0)
                {
                    PrintResultLine(
                        "Threw exceptions while formatting",
                        result.ExceptionsFormatting,
                        logger
                    );
                }

                if (result.ExceptionsValidatingSource != 0)
                {
                    PrintResultLine(
                        "files that threw exceptions while validating syntax tree",
                        result.ExceptionsValidatingSource,
                        logger
                    );
                }
            }

            if (commandLineOptions.Check)
            {
                PrintResultLine("files that were not formatted", result.UnformattedFiles, logger);
            }
        }

        private static void PrintResultLine(string message, int count, ILogger logger)
        {
            logger.LogInformation(PadToSize(message + ": ", 80) + ReversePad(count + "  "));
        }

        public static string PadToSize(string value, int size = 120)
        {
            if (value.Length >= size)
            {
                return value;
            }

            return value + new string(' ', size - value.Length);
        }

        public static string ReversePad(string value)
        {
            const int size = 10;
            if (value.Length >= size)
            {
                return value;
            }

            return new string(' ', size - value.Length) + value;
        }
    }
}
