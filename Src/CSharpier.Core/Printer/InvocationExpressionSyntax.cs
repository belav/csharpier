using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 1 when we chain method calls, they format weird. we want something like
        /*
        this.Where(o => o.lkasdfjlkasdfk)
            .OrderBy(o => o.jlasdkfjaslkdfkalsdf)
            .ThenBy(o => o.jlaksdfjkaslkfksldf)
         */
        private Doc PrintInvocationExpressionSyntax(InvocationExpressionSyntax node)
        {
            return Concat(this.Print(node.Expression), this.Print(node.ArgumentList));
        }
    }
}
