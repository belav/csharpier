namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchExpression
{
    public static Doc Print(SwitchExpressionSyntax node, FormattingContext context)
    {
        // TODO also look at keeping stuff with =>, see throw new
        /*
public class ClassName {
    public bool Foo(object entry)
    {
        return entry switch
        {
            string s => s.Length switch
            {
                1 => true,
                2 => false,
                _ => throw new ArgumentOutOfRangeException(
                    "this specific string length is not supported"
                ),
            },
            int i => i,
            _ => throw new ArgumentOutOfRangeException(
                $"entry type {entry.GetType()} not supported"
            ),
        };
    }
    
    also this one with comments
    
    private static bool TryAddEqualizedFieldsForStatements(
            IEnumerable<IOperation> statementsToCheck,
            ISymbol otherC,
            INamedTypeSymbol type,
            ArrayBuilder<IFieldSymbol> builder
        ) =>
            statementsToCheck.FirstOrDefault() switch
            {
                IReturnOperation
                {
                    ReturnedValue: ILiteralOperation
                    {
                        ConstantValue.HasValue: true,
                        ConstantValue.Value: true,
                    }
                }
                    // we are done with the comparison, the final statment does no checks
                    => true,
                IReturnOperation { ReturnedValue: IOperation value }
                    => TryAddEqualizedFieldsForCondition(
                        value,
                        successRequirement: true,
                        currentObject: type,
                        otherObject: otherC,
                        builder: builder
                    ),
                IConditionalOperation
                {
                    Condition: IOperation condition,
                    WhenTrue: IOperation whenTrue,
                    WhenFalse: var whenFalse,
                }
                    // 1. Check structure of if statment, get success requirement
                    // and any potential statments in the non failure block
                    // 2. Check condition for compared members
                    // 3. Check remaining members in non failure block
                    => TryGetSuccessCondition(
                        whenTrue,
                        whenFalse,
                        statementsToCheck.Skip(1),
                        out var successRequirement,
                        out var remainingStatements
                    )
                        && TryAddEqualizedFieldsForCondition(
                            condition,
                            successRequirement,
                            type,
                            otherC,
                            builder
                        )
                        && TryAddEqualizedFieldsForStatements(
                            remainingStatements,
                            otherC,
                            type,
                            builder
                        ),
                _ => false
            };
}
         */
        var sections = Doc.Group(
            Doc.Indent(
                Doc.HardLine,
                SeparatedSyntaxList.Print(
                    node.Arms,
                    (o, _) =>
                    {
                        var groupId1 = Guid.NewGuid().ToString();
                        var groupId2 = Guid.NewGuid().ToString();
                        return Doc.Concat(
                            ExtraNewLines.Print(o),
                            Token.PrintLeadingTrivia(
                                o.Pattern.GetLeadingTrivia(),
                                context.WithSkipNextLeadingTrivia()
                            ),
                            Doc.Group(
                                Doc.GroupWithId(
                                    groupId1,
                                    Doc.Concat(
                                        Node.Print(o.Pattern, context),
                                        o.WhenClause != null
                                            ? Node.Print(o.WhenClause, context)
                                            : Doc.Null
                                    )
                                ),
                                Doc.Concat(
                                    " ",
                                    Token.Print(o.EqualsGreaterThanToken, context),
                                    Doc.GroupWithId(groupId2, Doc.Indent(Doc.Line)),
                                    Doc.IndentIfBreak(Node.Print(o.Expression, context), groupId2)
                                )
                            )
                        );
                    },
                    Doc.HardLine,
                    context,
                    trailingSeparator: TrailingComma.Print(node.CloseBraceToken, context)
                )
            ),
            Doc.HardLine
        );

        DocUtilities.RemoveInitialDoubleHardLine(sections);

        return Doc.Concat(
            Node.Print(node.GoverningExpression, context),
            " ",
            Token.Print(node.SwitchKeyword, context),
            Doc.HardLine,
            Token.Print(node.OpenBraceToken, context),
            sections,
            Token.Print(node.CloseBraceToken, context)
        );
    }
}
