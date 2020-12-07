import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TryStatementNode extends SyntaxTreeNode<"TryStatement"> {}

export const print: PrintMethod<TryStatementNode> = (path, options, print) => {
    return "TODO TryStatement";
};
