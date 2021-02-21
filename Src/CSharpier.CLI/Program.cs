using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpier.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.CLI
{
    class Program
    {
        private static int sourceLost;
        private static int exceptionsFormatting;
        private static int exceptionsValidatingSource;
        private static int files;
        
        static void Main(string[] args)
        {
            // TODO 1 Configuration.cs from data.entities
            // TODO 1 CurrencyDto.cs from data.entities
            var fullStopwatch = Stopwatch.StartNew();
            //var path = "C:\\temp\\clifiles";
            //var path = @"C:\Projects\Newtonsoft.Json";
            //var path = @"C:\Projects\epi-identity";
            var path = @"C:\Projects\formattingTests\insite-commerce-prettier";
            // TODO 0 why does that weird file in roslyn fail validation?
            // also what about the files that fail to compile?
            //var path = @"C:\Projects\roslyn";
            // TODO 0 lots fail to compile, codegen files should be excluded perhaps?
            //var path = @"C:\Projects\aspnetcore";
            // TODO 0 stackoverflows, probably because of the ridiculous testing files that are in there, and c# thinks there is a stackoverflow because the stack is so large, and looks like it is repeating.
            // actually it looks like runtime has tests that will fail in recursive tree walkers
            // see https://github.com/dotnet/runtime/blob/master/src/tests/JIT/Regression/JitBlue/GitHub_10215/GitHub_10215.cs
            // TODO 0 also some weird "failures" due to trivia moving lines, although the compiled code would be the same
            //var path = @"C:\Projects\runtime";
            //var path = @"C:\Projects\core";
            //var path = @"C:\Projects\runtime\src\tests\Loader\classloader\generics\Instantiation\Nesting";
            //var path = @"C:\Projects\insite-commerce-prettier\Legacy";

            // TODO 0 we can also look at prettier, they do some stuff like run it twice and compare AST, compare file to make sure the 2nd run doesn't change it from the first run, etc
            // not sure how the AST compare will work because we are modifying leading/trailing trivia, unless we compare everything except whitespace/endofline trivia
            // seems like way more work than my current naive approach
            
            var tasks = Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories).Select(o => DoWork(o, path)).ToArray();
            Task.WaitAll(tasks);
            Console.WriteLine(PadToSize("total time: ", 80) + ReversePad(fullStopwatch.ElapsedMilliseconds + "ms"));
            Console.WriteLine(PadToSize("total files: ", 80) + ReversePad(files.ToString()));
            Console.WriteLine(PadToSize("files that failed syntax tree validation: ", 80) + ReversePad(sourceLost.ToString()));
            Console.WriteLine(PadToSize("files that threw exceptions while formatting: ", 80) + ReversePad(exceptionsFormatting.ToString()));
            Console.WriteLine(PadToSize("files that threw exceptions while validating syntax tree: ", 80) + ReversePad(exceptionsValidatingSource.ToString()));
        }

        private static async Task DoWork(string file, string path)
        {
            Console.WriteLine(file);
            if (file.EndsWith(".g.cs"))
            {
                return;
            }

            files++;
            
            using var reader = new StreamReader(file);
            var code = await reader.ReadToEndAsync();
            var encoding = reader.CurrentEncoding;
            reader.Close();
            var formatter = new CodeFormatter();

            CSharpierResult result;

            try
            {
                result = formatter.Format(code, new Options
                {
                    TestRun = true,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(file.Substring(path.Length) + " - threw exception while formatting");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                exceptionsFormatting++;
                return;
            }


            if (!string.IsNullOrEmpty(result.Errors))
            {
                Console.WriteLine(file.Substring(path.Length) + " - failed to compile");
                return;
            }

            // TODO 1 use async inside of codeformatter?
            var syntaxNodeComparer = new SyntaxNodeComparer(code, result.Code);
            var paddedFile = PadToSize(file.Substring(path.Length));
            
            try
            {
                var failure = syntaxNodeComparer.CompareSource();
                if (!string.IsNullOrEmpty(failure))
                {
                    sourceLost++;
                    Console.WriteLine(paddedFile + " - failed syntax tree validation");
                    Console.WriteLine(failure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(paddedFile + " - failed with exception during syntax tree validation" + Environment.NewLine + ex.Message + ex.StackTrace);
                exceptionsValidatingSource++;
            }
            
            await File.WriteAllBytesAsync(file, encoding.GetBytes(result.Code));
        }
        
        private static string PadToSize(string value, int size = 120)
        {
            while (value.Length < size)
            {
                value += " ";
            }

            return value;
        }

        private static string ReversePad(string value)
        {
            while (value.Length < 10)
            {
                value = " " + value;
            }

            return value;
        }
    }
}