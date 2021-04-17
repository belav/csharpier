using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CSharpier.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenchmarkFormatting>();
        }
    }

    // 48us with no changes
    public class BenchmarkFormatting
    {
        [Benchmark]
        public void FormatCode()
        {
            var code =
                @"public class ClassName {
    public void LongUglyMethod(string longParameter1, string longParameter2, string longParameter3) { 
    }
}";
            var result = new CodeFormatter().Format(
                code,
                new Options { IncludeDocTree = false, IncludeAST = false, }
            );
        }
    }
}
