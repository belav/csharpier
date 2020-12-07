import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UnaryMinusExpressionNode extends SyntaxTreeNode<"UnaryMinusExpression"> {}

export const print: PrintMethod<UnaryMinusExpressionNode> = (path, options, print) => {
    return "TODO UnaryMinusExpression";
};
