import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { Operator, printPathValue } from "./SyntaxTreeNode";

export function print<T extends Operator>(path: FastPath<T>, options: ParserOptions<T>, print: Print<T>) {
    return concat([printPathValue(path, "operatorToken"), path.call(print, "operand")]);
}
