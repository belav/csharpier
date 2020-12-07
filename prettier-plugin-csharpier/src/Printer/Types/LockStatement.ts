import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LockStatementNode extends SyntaxTreeNode<"LockStatement"> {}

export const print: PrintMethod<LockStatementNode> = (path, options, print) => {
    return "TODO LockStatement";
};
