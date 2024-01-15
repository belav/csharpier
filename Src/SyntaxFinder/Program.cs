using Microsoft.CodeAnalysis.CSharp;
using SyntaxFinder;

var files = Directory.EnumerateFiles(
    "C:\\projects\\csharpier-repos",
    "*.cs",
    SearchOption.AllDirectories
);

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

        var syntaxWalker = new ObjectInitializerWalker(file);
        syntaxWalker.Visit(tree.GetRoot());
    }
);

ObjectInitializerWalker.WriteResult();
