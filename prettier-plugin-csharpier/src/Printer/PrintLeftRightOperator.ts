import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

export interface LeftRightOperator {
    left: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    right: SyntaxTreeNode;
}

// TODO 0 the rawKind doesn't line up to the type in c#, maybe we should move away from that. that would cut out a lot of code
// everything that uses this print ends up being BinaryExpressionSyntax, AssignmentExpressionSyntax or BinaryPatternSyntax
export function printLeftRightOperator<T extends LeftRightOperator>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([
        path.call(print, "left"),
        " ",
        printPathSyntaxToken(path, "operatorToken"),
        " ",
        path.call(print, "right"),
    ]);
}
