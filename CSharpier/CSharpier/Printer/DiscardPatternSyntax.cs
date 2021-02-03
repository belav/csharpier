using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDiscardPatternSyntax(DiscardPatternSyntax node)
        {
            return String("_");
        }
    }
}
