#pragma warning disable RSEXPERIMENTAL006

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace CSharpier.Generators;

[Generator]
public class PowershellNodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxTrees = context.CompilationProvider.Select(
            (compilation, _) =>
                compilation
                    .SyntaxTrees.Where(o => o.FilePath.Contains("AstPrinters"))
                    .ToImmutableArray()
        );

        context.RegisterSourceOutput(syntaxTrees, GenerateSource);
    }

    private static void GenerateSource(
        SourceProductionContext context,
        ImmutableArray<SyntaxTree> syntaxTrees
    )
    {
        if (syntaxTrees.Length == 0)
        {
            return;
        }

        var generator = new PowershellNodeGenerator();
        var template = Template.Parse(generator.GetContent(generator.GetType().Name + ".sbntxt"));
        var renderedSource = template.Render(
            generator.GetModel(syntaxTrees),
            member => member.Name
        );

        var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

        context.AddSource("Node", sourceText);
    }

    public string GetContent(string relativePath)
    {
        var assembly = this.GetType().Assembly;
        var baseName = assembly.GetName().Name;
        var resourceName = relativePath
            .TrimStart('.')
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');

        var name = baseName + "." + resourceName;
        using var stream = assembly.GetManifestResourceStream(name);

        if (stream == null)
        {
            var list = assembly.GetManifestResourceNames();

            throw new Exception(
                $"No embedded resource found with the name {name}. Resources found are "
                    + string.Join(", ", list)
            );
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    // The switch in Node.Print matches the first arm that fits, so a printer for a derived Ast type
    // has to come before the printer for its base type. The generator only sees file names, so the
    // base types that matter are listed here.
    private static readonly Dictionary<string, string> BaseTypes = new()
    {
        ["StringConstantExpression"] = "ConstantExpression",
        ["InvokeMemberExpression"] = "MemberExpression",
        ["BaseCtorInvokeMemberExpression"] = "InvokeMemberExpression",
        ["ConvertExpression"] = "AttributedExpression",
        ["ConstantExpression"] = "Expression",
        ["MemberExpression"] = "Expression",
        ["AttributedExpression"] = "Expression",
        ["Attribute"] = "AttributeBase",
        ["TypeConstraint"] = "AttributeBase",
        ["PropertyMember"] = "Member",
        ["FunctionMember"] = "Member",
        ["FileRedirection"] = "Redirection",
        ["MergingRedirection"] = "Redirection",
        ["Pipeline"] = "PipelineBase",
        ["PipelineChain"] = "PipelineBase",
        ["AssignmentStatement"] = "PipelineBase",
        ["ErrorStatement"] = "PipelineBase",
        ["PipelineBase"] = "Statement",
        ["Command"] = "CommandBase",
        ["CommandExpression"] = "CommandBase",
        ["CommandBase"] = "Statement",
        ["ForStatement"] = "LoopStatement",
        ["ForEachStatement"] = "LoopStatement",
        ["WhileStatement"] = "LoopStatement",
        ["DoWhileStatement"] = "LoopStatement",
        ["DoUntilStatement"] = "LoopStatement",
        ["LoopStatement"] = "LabeledStatement",
        ["SwitchStatement"] = "LabeledStatement",
        ["LabeledStatement"] = "Statement",
        ["Expression"] = "CommandElement",
        ["CommandParameter"] = "CommandElement",
    };

    private static int GetInheritanceDepth(string printerName)
    {
        var depth = 0;
        while (true)
        {
            if (!BaseTypes.TryGetValue(printerName, out var baseType))
            {
                if (printerName.Length > "Expression".Length && printerName.EndsWith("Expression"))
                {
                    baseType = "Expression";
                }
                else if (
                    printerName.Length > "Statement".Length
                    && printerName.EndsWith("Statement")
                )
                {
                    baseType = "Statement";
                }
                else
                {
                    break;
                }
            }

            depth++;
            printerName = baseType;
        }

        return depth;
    }

    private object GetModel(ImmutableArray<SyntaxTree> syntaxTrees)
    {
        var nodeTypes = syntaxTrees
            .Select(o => Path.GetFileNameWithoutExtension(o.FilePath))
            .Select(fileName => new
            {
                PrinterName = fileName,
                SyntaxNodeName = $"{fileName}Ast",
                VariableName = char.ToLower(fileName[0]) + fileName[1..],
            })
            .OrderByDescending(o => GetInheritanceDepth(o.PrinterName))
            .ThenBy(o => o.SyntaxNodeName)
            .ToArray();

        return new { NodeTypes = nodeTypes };
    }
}
