import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LockStatementNode extends SyntaxTreeNode<"LockStatement"> {
    lockKeyword: SyntaxToken;
    expression: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const printLockStatement: PrintMethod<LockStatementNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "lockKeyword"),
        " (",
        path.call(print, "expression"),
        ")",
        path.call(print, "statement"),
    ]);
};
