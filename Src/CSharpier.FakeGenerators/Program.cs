using CSharpier.FakeGenerators;

// source generators use the version of Microsoft.CodeAnalysis.CSharp of the version of dotnet that is
// executing the source generator, not the version that is referenced in the source generator project
// this leads to pain and these files only need to be regenerated if
// we change the version of Microsoft.CodeAnalysis.CSharp referenced

// for example modify global.json to use 6.0.300
// CSharpier.Generators references Microsoft.CodeAnalysis.CSharp 4.1.0
// execute these as actual generators
// the code is generated with the types in Microsoft.CodeAnalysis.CSharp 4.2.0 (from 6.0.300)
// the code is used in CSharpier which is also on Microsoft.CodeAnalysis.CSharp 4.1.0
// CSharpier will not compile

var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
while (!directory.Name.Equals("Src", StringComparison.Ordinal))
{
    directory = directory.Parent;
    if (directory == null)
    {
        throw new Exception(
            "Could not find the directory Src above the directory "
                + Directory.GetCurrentDirectory()
        );
    }
}

var codeContext = new CodeContext(Path.Combine(directory.FullName, "CSharpier.Core", "CSharp"));
SyntaxNodeComparerGenerator.Execute(codeContext);
new SyntaxNodeJsonWriterGenerator().Execute(codeContext);
