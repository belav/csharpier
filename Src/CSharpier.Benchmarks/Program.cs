using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using CSharpier.Benchmarks;

var config = DefaultConfig.Instance.WithArtifactsPath(
    Path.Combine(Paths.RepoRoot, "Src/CSharpier.Benchmarks/BenchmarkDotNet.Artifacts")
);

BenchmarkSwitcher.FromAssembly(typeof(CliBenchmarks).Assembly).Run(args, config);
