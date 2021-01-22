import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EmptyStatementNode extends SyntaxTreeNode<"EmptyStatement"> {}

export const printEmptyStatement: PrintMethod<EmptyStatementNode> = (path, options, print) => {
    return ";";
};
