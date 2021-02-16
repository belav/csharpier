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
        static void Main(string[] args)
        {
            // TODO 1 Configuration.cs from data.entities
            // TODO 1 CurrencyDto.cs from data.entities
            var fullStopwatch = Stopwatch.StartNew();
            //var path = "C:\\temp\\clifiles";
            var path = @"C:\Projects\insite-commerce-prettier\Legacy\Automation\Insite.Automated.Core";

            // TODO 0 we can also look at prettier, they do some stuff like run it twice and compare AST, compare file to make sure the 2nd run doesn't change it from the first run, etc
            // not sure how the AST compare will work because we are modifying leading/trailing trivia, unless we compare everything except whitespace/endofline trivia
            // seems like way more work than my current naive approach

            var tasks = Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories).AsParallel().Select(o => DoWork(o, path)).ToArray();
            Task.WaitAll(tasks);
            Console.WriteLine("total: " + fullStopwatch.ElapsedMilliseconds + "ms");
        }

        private static async Task DoWork(string file, string path)
        {
            if (file.EndsWith(".g.cs"))
            {
                return;
            }

            using var reader = new StreamReader(file);
            var code = await reader.ReadToEndAsync();
            var encoding = reader.CurrentEncoding;
            reader.Close();
            var formatter = new CodeFormatter();
            var result = formatter.Format(code, new Options
            {
                TestRun = true,
            });

            if (!string.IsNullOrEmpty(result.Errors))
            {
                Console.WriteLine(file.Substring(path.Length) + " - failed to compile");
                return;
            }

            var syntaxNodeComparer = new SyntaxNodeComparer();
            // TODO 1 use async inside of codeformatter?
            var left = await CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.CSharp9)).GetRootAsync();
            var right = await CSharpSyntaxTree.ParseText(result.Code, new CSharpParseOptions(LanguageVersion.CSharp9)).GetRootAsync();

            var squashThinksEqual = await IsCodeBasicallyEqual(code, result.Code, file, path);

            var paddedFile = PadToSize(file.Substring(path.Length));

            try
            {
                var comparerResult = syntaxNodeComparer.AreEqualIgnoringWhitespace(left, right, "Root");
                if (!comparerResult.AreEqual)
                {
                    if (!squashThinksEqual)
                    {
                        Console.WriteLine(paddedFile + " - failed both!");
                    }
                    else
                    {
                        Console.WriteLine(paddedFile + " - failed comparer!");
                    }
                    Console.WriteLine("    " + comparerResult.MismatchedPath);
                }
                else if (!squashThinksEqual)
                {
                    Console.WriteLine(paddedFile + " - failed squash");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(paddedFile + " - failed with exception" + Environment.NewLine + ex.Message);
            }
            

            
            await File.WriteAllBytesAsync(file, encoding.GetBytes(result.Code));
        }
        
        private static string PadToSize(string value)
        {
            while (value.Length < 120)
            {
                value += " ";
            }

            return value;
        }
        
        public static async Task<bool> IsCodeBasicallyEqual(string code, string formattedCode, string file, string path)
        {
            var squashCode = Squash(code);
            var squashFormattedCode = Squash(formattedCode);
            var newFile = Path.Combine("c:/temp/testing",  file.Substring(path.Length).Replace("/", "_").Replace("\\", "_"));

            if (squashCode != squashFormattedCode)
            {
                await File.WriteAllTextAsync(newFile.Replace(".cs", ".org.cs"), squashCode);
                await File.WriteAllTextAsync(newFile.Replace(".cs", ".new.cs"), squashFormattedCode);
                return false;
            }

            return true;
        }

        private static string Squash(string code)
        {
            var result = new StringBuilder();
            for (var x = 0; x < code.Length; x++)
            {
                var nextChar = code[x];
                if (nextChar == ' ' || nextChar == '\t' || nextChar == '\r' || nextChar == '\n')
                {
                    if (result.Length == 0 || result[^1] != ' ')
                    {
                        result.Append(' ');
                    }
                }
                else
                {
                    result.Append(nextChar);
                }
            }

            return result.ToString()
                .Replace("( ", "(")
                .Replace(") ", ")")
                .Replace(" (", "(")
                .Replace(" )", ")")
                .Replace("{ ", "{")
                .Replace("} ", "}")
                .Replace(" {", "{")
                .Replace(" }", "}")
                .Replace("[ ", "[")
                .Replace("] ", "]")
                .Replace(" ]", "]")
                .Replace(" [", "[")
                .Replace(" .", ".")
                .Replace("< ", "<")
                .Replace("> ", ">")
                .Replace(" <", "<")
                .Replace(" >", ">")
                .Replace(", ", ",")
                .Replace(": ", ":")
                .Replace(" :", ":")
                .Replace("; ", ";")
                .Replace(" ;", ";")
                .TrimEnd(' ');
        }
    }
}