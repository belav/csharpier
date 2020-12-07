import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LabeledStatementNode extends SyntaxTreeNode<"LabeledStatement"> {}

export const print: PrintMethod<LabeledStatementNode> = (path, options, print) => {
    return "TODO LabeledStatement";
};
