using System.Collections;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CSharpier.Core.PowerShell;

// The PowerShell AST does not serialize cleanly - nodes point back at their Parent, which would
// cycle - so walk it by reflection, emitting each node's type and its child Asts/values as JSON.
internal static class AstSyntaxWriter
{
    private const int MaxDepth = 200;

    private static readonly JsonSerializerOptions IndentedJson = new() { WriteIndented = true };

    internal static string Write(Ast ast)
    {
        return JsonSerializer.Serialize(ToNode(ast, 0), IndentedJson);
    }

    private static JsonObject ToNode(Ast ast, int depth)
    {
        var node = new JsonObject { ["NodeType"] = ast.GetType().Name };

        if (depth >= MaxDepth)
        {
            return node;
        }

        foreach (
            var property in ast.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
        )
        {
            // Parent walks back up the tree; Extent repeats the source text on every node.
            if (property.Name is nameof(Ast.Parent) or nameof(Ast.Extent))
            {
                continue;
            }

            object? value;
            try
            {
                value = property.GetValue(ast);
            }
            catch
            {
                continue;
            }

            var converted = ToValue(value, depth + 1);
            if (converted is not null)
            {
                node[property.Name] = converted;
            }
        }

        return node;
    }

    private static JsonNode? ToValue(object? value, int depth)
    {
        switch (value)
        {
            case null:
                return null;
            case Ast ast:
                return ToNode(ast, depth);
            case string text:
                return text;
            case bool boolean:
                return boolean;
            case Enum enumValue:
                return enumValue.ToString();
            // A single string is IEnumerable<char>, so it is handled above before we get here.
            case IEnumerable enumerable:
                var array = new JsonArray();
                foreach (var item in enumerable)
                {
                    var converted = ToValue(item, depth);
                    if (converted is not null)
                    {
                        array.Add(converted);
                    }
                }

                return array.Count > 0 ? array : null;
            default:
                return value.ToString();
        }
    }
}
