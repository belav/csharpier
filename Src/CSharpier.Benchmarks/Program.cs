using BenchmarkDotNet.Running;
using CSharpier.Benchmarks;

if (args is { Length: > 0 })
{
    _ = BenchmarkSwitcher.FromAssembly(typeof(CSharpBenchmarks).Assembly).Run(args);
}
else
{
    _ = BenchmarkRunner.Run<CSharpBenchmarks>();
    // _ = BenchmarkRunner.Run<CliBenchmarks>();
    // _ = BenchmarkRunner.Run<XmlBenchmarks>();
}
