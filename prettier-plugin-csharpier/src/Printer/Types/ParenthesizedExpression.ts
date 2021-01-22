import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ParenthesizedExpressionNode extends SyntaxTreeNode<"ParenthesizedExpression"> {
    expression: SyntaxTreeNode;
}

export const printParenthesizedExpression: PrintMethod<ParenthesizedExpressionNode> = (path, options, print) => {
    return concat(["(", path.call(print, "expression"), ")"]);
};
