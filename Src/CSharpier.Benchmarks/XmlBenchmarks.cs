using System.Xml;
using System.Xml.Linq;
using BenchmarkDotNet.Attributes;
using CSharpier.Core;
using CSharpier.Core.Xml;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class XmlBenchmarks
{
    [Benchmark]
    public void XmlDocument_Parse()
    {
        var root = new XmlDocument();
        root.LoadXml(this.largeXmlCode);
    }

    [Benchmark]
    public void XDocument_Parse()
    {
        _ = XDocument.Parse(this.largeXmlCode);
    }

    [Benchmark]
    public void CustomParser_Parse()
    {
        _ = RawNodeReader.ParseXml(
            this.largeXmlCode,
            Environment.NewLine,
            XmlWhitespaceSensitivity.Strict
        );
    }

    [Benchmark]
    public void XmlReader_Parse()
    {
        using var xmlReader = XmlReader.Create(
            new StringReader(this.largeXmlCode),
            new XmlReaderSettings { IgnoreWhitespace = false }
        );

        while (xmlReader.Read())
        {
            //
        }
    }

    private readonly string largeXmlCode = File.ReadAllText(
        Path.Combine(Paths.RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/Type.xml")
    );
}
