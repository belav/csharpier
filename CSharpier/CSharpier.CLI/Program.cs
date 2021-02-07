using System;
using System.IO;

namespace CSharpier.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("C:\\Projects\\csharpier\\CSharpier\\CSharpier.Tests\\Samples\\AllInOne.cst");
            var formatter = new CodeFormatter();
            var result = formatter.Format(code, new Options());
            Console.Write(result);
        }
    }
}