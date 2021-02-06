using System;
using System.IO;

namespace CSharpier.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("C:\\Projects\\csharpier\\CSharpier\\CSharpier.Tests\\Samples\\AllInOne.cst");
            var formatter = new Formatter();
            var result = formatter.MakeCodeCSharpier(code, new Options());
            Console.Write(result);
        }
    }
}