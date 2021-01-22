import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { Operator, printPathSyntaxToken } from "./SyntaxTreeNode";

export function printPreOperator<T extends Operator>(path: FastPath<T>, options: ParserOptions<T>, print: Print<T>) {
    return concat([printPathSyntaxToken(path, "operatorToken"), path.call(print, "operand")]);
}
