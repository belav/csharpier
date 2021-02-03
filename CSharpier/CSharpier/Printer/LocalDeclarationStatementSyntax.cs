using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLocalDeclarationStatementSyntax(LocalDeclarationStatementSyntax node)
        {
            var parts = new Parts();
            // TODO 0 this is SLOW, see https://github.com/prettier/prettier/issues/4459 for async progress
            // also just figure out what it is doing that is so slow? Could the parser cache results? Could this cache the results of calling the parser?
            // should I redo this to get rid of some of the abstractions that make my life easier to speed up things?
            // TODO 0 should we make methods for things like declaration? so we don't have to put all these crazy things here
            // in our main print, we look up something fot he node and call
            // node.PrintLeadingComments, node.Print, node.PrintTrailingComments
            // TODO 0 the other option is to do it more like prettier-java, we just look for comments on everything, and prepend/append them and let prettier format it with the comments where they used to be
            // TODO 0 test that out next! make the print Lines/comments stuff do nothing, and see if I can get the tests passing with more generic stuff.
            // TODO 0 the visit pattern https://github.com/prettier/prettier/issues/5747
            // looks like it depends on a class from the parser, I'm not sure if I could do it exactly, plus I do need access to the parent in at least one place
            // printExtraNewLines(
            //     node,
            //     parts,
            //     String("awaitKeyword"),
            //     String("usingKeyword"),
            //     String("modifiers"),
            //     [String("declaration"), String("type"), String("keyword")],
            //     [String("declaration"), String("type"), String("identifier")]
            // );
            // printLeadingComments(
            //     node,
            //     parts,
            //     String("awaitKeyword"),
            //     String("usingKeyword"),
            //     String("modifiers"),
            //     [String("declaration"), String("type"), String("keyword")],
            //     [String("declaration"), String("type"), String("identifier")]
            // );
            if (NotNull(node.AwaitKeyword)) {
                parts.Push(String("await "));
            }
            if (NotNull(node.UsingKeyword)) {
                parts.Push(String("using "));
            }
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintVariableDeclarationSyntax(node.Declaration));
            parts.Push(String(";"));
            // TODO printTrailingComments(node, parts, String("semicolonToken"));
            return Concat(parts);
        }
    }
}
