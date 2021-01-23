import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

export interface LeftRightOperator {
    left: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    right: SyntaxTreeNode;
}

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
