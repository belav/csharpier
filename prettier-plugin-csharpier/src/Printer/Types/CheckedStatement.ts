import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CheckedStatementNode extends SyntaxTreeNode<"CheckedStatement"> {

}

export const print: PrintMethod<CheckedStatementNode> = (path, options, print) => {
    return "TODO CheckedStatement";
};
