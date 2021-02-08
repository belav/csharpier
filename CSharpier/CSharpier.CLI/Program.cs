using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var fullStopwatch = Stopwatch.StartNew();
            //var path = "C:\\temp\\clifiles";
            var path = "C:\\Projects\\insite-commerce-prettier";


            Parallel.ForEach(Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories), async (string file) =>
            {
                var code = await File.ReadAllTextAsync(file);
                var stopwatch = Stopwatch.StartNew();
                var formatter = new CodeFormatter();
                var result = formatter.Format(code, new Options());
                Console.WriteLine(file.Substring(path.Length) + ": " + stopwatch.ElapsedMilliseconds + "ms");
                await File.WriteAllTextAsync(file, result.Code, new UTF8Encoding(false));
            });
            Console.WriteLine("total: " + fullStopwatch.ElapsedMilliseconds + "ms");
        }
    }
}