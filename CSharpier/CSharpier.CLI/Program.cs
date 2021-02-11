using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpier.Core;
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
            //var path = @"C:\Projects\insite-commerce-prettier\Legacy\Data";
            var path = @"c:\projects\mission-control";

            // TODO we can also look at prettier, they do some stuff like run it twice and compare AST, compare file to make sure the 2nd run doesn't change it from the first run, etc
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

            if (!IsCodeBasicallyEqual(code, result.Code, file))
            {
                Console.WriteLine(file.Substring(path.Length) + " - failed!");
            }

            await File.WriteAllBytesAsync(file, encoding.GetBytes(result.Code));
        }
        
        public static bool IsCodeBasicallyEqual(string code, string formattedCode, string file)
        {
            var squashCode = Squash(code);
            var squashFormattedCode = Squash(formattedCode);
            if (squashCode != squashFormattedCode)
            {
                File.WriteAllText(file.Replace(".cs", ".org.cs"), squashCode);
                File.WriteAllText(file.Replace(".cs", ".new.cs"), squashFormattedCode);
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
                .TrimEnd(' ');
        }
    }
}