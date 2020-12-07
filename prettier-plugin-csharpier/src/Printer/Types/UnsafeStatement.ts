import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UnsafeStatementNode extends SyntaxTreeNode<"UnsafeStatement"> {}

export const print: PrintMethod<UnsafeStatementNode> = (path, options, print) => {
    return "TODO UnsafeStatement";
};
