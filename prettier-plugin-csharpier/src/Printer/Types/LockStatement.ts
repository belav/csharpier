import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LockStatementNode extends SyntaxTreeNode<"LockStatement"> {
    lockKeyword: HasValue;
    expression: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const print: PrintMethod<LockStatementNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "lockKeyword"),
        " (",
        path.call(print, "expression"),
        ")",
        path.call(print, "statement"),
    ]);
};
