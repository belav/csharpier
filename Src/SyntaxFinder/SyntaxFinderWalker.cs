namespace SyntaxFinder;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public abstract class SyntaxFinderWalker : CSharpSyntaxWalker
{
    private bool wroteFile;
    private readonly int maxCodeWrites = 250;
    private static int codeWrites = 0;
    private readonly string file;

    protected SyntaxFinderWalker(string file)
    {
        this.file = file;
    }

    protected void WriteCode(SyntaxNode syntaxNode)
    {
        if (codeWrites < this.maxCodeWrites)
        {
            Interlocked.Increment(ref codeWrites);
            Console.WriteLine(syntaxNode.SyntaxTree.GetText().ToString(syntaxNode.FullSpan));
            Console.WriteLine();
        }
    }

    protected void WriteFilePath(bool onlyIfWritingCode = false)
    {
        if (onlyIfWritingCode && codeWrites >= this.maxCodeWrites)
        {
            return;
        }

        if (!this.wroteFile)
        {
            Console.WriteLine(this.file);
            this.wroteFile = true;
        }
    }
}
