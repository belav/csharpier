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

            // TODO use test run stuff in here, maybe start with smaller directories to track down issues
            // or only write changes for fails, so it is easy to find them.

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