using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using SyntaxFinder;

var files = Directory.EnumerateFiles(
    "C:\\projects\\csharpier-repos",
    "*.cs",
    SearchOption.AllDirectories
);

var typesByName = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

Console.WriteLine("Which type do you want to run?");
var x = 1;
foreach (var type in typeof(SyntaxFinderWalker).Assembly.GetTypes())
{
    if (typeof(CSharpSyntaxWalker).IsAssignableFrom(type) && !type.IsAbstract)
    {
        typesByName.Add(x.ToString(), type);
        Console.WriteLine(x + " " + type.Name);
        x++;
    }
}

var nameOfTypeToRun = Console.ReadLine() ?? string.Empty;

if (!typesByName.TryGetValue(nameOfTypeToRun, out var typeToRun))
{
    throw new Exception($"{nameOfTypeToRun} was not in the list");
}

var constructor = typeToRun.GetConstructors().Single();

Parallel.ForEach(
    files,
    file =>
    {
        if (Ignored.Is(file))
        {
            return;
        }

        var contents = File.ReadAllText(file);
        var tree = CSharpSyntaxTree.ParseText(contents);

        var syntaxWalker = constructor.Invoke([file]) as CSharpSyntaxWalker;
        syntaxWalker!.Visit(tree.GetRoot());
    }
);

typeToRun.GetMethod("WriteResult")?.Invoke(null, null);
