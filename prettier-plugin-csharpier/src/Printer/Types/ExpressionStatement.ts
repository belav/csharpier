import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ExpressionStatementNode extends SyntaxTreeNode<"ExpressionStatement"> {
    expression: SyntaxTreeNode;
}

export const printExpressionStatement: PrintMethod<ExpressionStatementNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), ";"]);
};
