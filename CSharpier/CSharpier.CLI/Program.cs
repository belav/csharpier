using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CSharpier.Core;

namespace CSharpier.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var fullStopwatch = Stopwatch.StartNew();
            //var path = "C:\\temp\\clifiles";
            var path = "C:\\Projects\\insite-commerce-prettier";

            // TODO can I use this? https://stackoverflow.com/questions/34945023/roslyn-syntaxtree-diff
            // TODO use test run stuff in here, maybe start with smaller directories to track down issues
            // or only write changes for fails, so it is easy to find them.
            // TODO we can also look at prettier, they do some stuff like run it twice and compare AST, compare file to make sure the 2nd run doesn't change it from the first run, etc
            // not sure how the AST compare will work because we are modifying leading/trailing trivia, unless we compare everything except whitespace/endofline trivia
            // hmmmm
            // seems like way more work than my current naive approach

            Parallel.ForEach(Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories), async (string file) =>
            {
                var code = await File.ReadAllTextAsync(file);
                var stopwatch = Stopwatch.StartNew();
                var formatter = new CodeFormatter();
                var result = formatter.Format(code, new Options());
                //Console.WriteLine(file.Subpath.Length + ": " + stopwatch.ElapsedMilliseconds + "ms");
                await File.WriteAllTextAsync(file, result.Code, new UTF8Encoding(false));
            });
            Console.WriteLine("total: " + fullStopwatch.ElapsedMilliseconds + "ms");
        }
    }
}