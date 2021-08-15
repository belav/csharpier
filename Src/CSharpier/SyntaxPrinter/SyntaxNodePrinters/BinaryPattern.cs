using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BinaryPattern
    {
        public static Doc Print(BinaryPatternSyntax node)
        {
            return Doc.IndentIf(
                node.Parent is SubpatternSyntax,
                Doc.Concat(
                    Node.Print(node.Left),
                    Doc.Line,
                    Token.PrintWithSuffix(node.OperatorToken, " "),
                    Node.Print(node.Right)
                )
            );
        }
    }
}
/*

// used to break before is
            if (
                contractResolver.ResolveContract(
                    target.GetType()
                ) is JsonObjectContract jsonObjectContract
            ) {
// related
                        if (
                            GetModelStateValue(
                                viewContext,
                                fullName,
                                typeof(string)
                            ) is string modelStateValue
                        ) {

                            if (
                                node.Children[
                                    i
                                ] is CSharpExpressionIntermediateNode nestedExpression
                            ) {
                            
            if (
                TestSink.Writes.FirstOrDefault(
                    w => w.LogLevel > LogLevel.Information
                ) is WriteContext writeContext
            ) {

// this is over the limit
                        if (
                            duplicate is TagHelperDirectiveAttributeIntermediateNode duplicateDirectiveAttribute
                        ) {

// what should this do?
if (
    someOtherValue_____________
    is (SomeLongType_____________________________________
        or SomeLongType_____________________________________)
) {
    return;
}

// this is uh, ugly, maybe just don't indent the { } ?
// also the new break fore is makes this version uglier, maybe break before is should only be for variable stuff?
if (
    !(
        node is PrefixUnaryExpressionSyntax
            {
                Operand: ParenthesizedExpressionSyntax
                    {
                        Expression: IsPatternExpressionSyntax
                            {
                                Pattern: DeclarationPatternSyntax,
                            } isPattern,
                    },
            } notExpression
    )
) {
    return;
}

// could this be improved??
    return someValue switch
    {
        OrEvenSomeOtherObject_________________
          => CallSomeMethod(someValue),
        SomeOtherObject
        {
            SomeProperty: SomeOtherProject
        }
        or AnotherObject
          => CallSomeMethod(someValue),
        AnotherObject
        or SomeOtherObject
        {
            SomeProperty: SomeOtherProject
        }
          => CallSomeMethod(someValue)
    }
*/
