import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "./SyntaxTreeNode";

export interface LeftRightOperator {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export function print<T extends LeftRightOperator>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([
        path.call(print, "left"),
        " ",
        printPathValue(path, "operatorToken"),
        " ",
        path.call(print, "right"),
    ]);
}
