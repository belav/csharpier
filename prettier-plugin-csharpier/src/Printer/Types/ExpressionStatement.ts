import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ExpressionStatementNode extends SyntaxTreeNode<"ExpressionStatement"> {
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<ExpressionStatementNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), ";"]);
};
