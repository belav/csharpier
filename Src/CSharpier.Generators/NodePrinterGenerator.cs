using System.IO;
using System.Linq;
using Generators;
using Microsoft.CodeAnalysis;

namespace CSharpier.Generators
{
    [Generator]
    public class NodePrinterGenerator : TemplatedGenerator
    {
        protected override string SourceName => "Node";

        protected override object GetModel(GeneratorExecutionContext context)
        {
            var nodeTypes = context.Compilation.SyntaxTrees.Where(
                    o => o.FilePath.Contains("SyntaxNodePrinters")
                )
                .Select(o => Path.GetFileNameWithoutExtension(o.FilePath))
                .Select(
                    fileName =>
                        new
                        {
                            PrinterName = fileName,
                            SyntaxNodeName = fileName + "Syntax",
                            VariableName = fileName[0].ToString().ToLower() + fileName[1..]
                        }
                )
                .OrderBy(o => o.SyntaxNodeName)
                .ToArray();

            return new { NodeTypes = nodeTypes };
        }
    }
}
