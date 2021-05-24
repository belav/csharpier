namespace CSharpier
{
    public class ResultPrinter
    {
        public static void PrintResults(
            CommandLineFormatterResult result,
            IConsole console,
            CommandLineOptions commandLineOptions
        ) {
            console.WriteLine(
                PadToSize("total time: ", 80) + ReversePad(result.ElapsedMilliseconds + "ms")
            );
            PrintResultLine("Total files", result.Files, console);

            if (!commandLineOptions.Fast)
            {
                PrintResultLine(
                    "Failed syntax tree validation",
                    result.FailedSyntaxTreeValidation,
                    console
                );

                PrintResultLine(
                    "Threw exceptions while formatting",
                    result.ExceptionsFormatting,
                    console
                );
                PrintResultLine(
                    "files that threw exceptions while validating syntax tree",
                    result.ExceptionsValidatingSource,
                    console
                );
            }

            if (commandLineOptions.Check)
            {
                PrintResultLine("files that were not formatted", result.UnformattedFiles, console);
            }
        }

        private static void PrintResultLine(string message, int count, IConsole console)
        {
            console.WriteLine(PadToSize(message + ": ", 80) + ReversePad(count + "  "));
        }

        public static string PadToSize(string value, int size = 120)
        {
            return value + new string(' ', size - value.Length);
        }

        public static string ReversePad(string value)
        {
            return new string(' ', 10 - value.Length) + value;
        }
    }
}
