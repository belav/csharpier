using BenchmarkDotNet.Running;
using CSharpier.Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(CliBenchmarks).Assembly).Run(args);
