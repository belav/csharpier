import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { Operator, printPathSyntaxToken } from "./SyntaxTreeNode";

export function printPostOperator<T extends Operator>(path: FastPath<T>, options: ParserOptions<T>, print: Print<T>) {
    return concat([path.call(print, "operand"), printPathSyntaxToken(path, "operatorToken")]);
}
