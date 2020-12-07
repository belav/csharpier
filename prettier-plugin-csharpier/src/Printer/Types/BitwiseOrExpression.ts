import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BitwiseOrExpressionNode extends SyntaxTreeNode<"BitwiseOrExpression"> {}

export const print: PrintMethod<BitwiseOrExpressionNode> = (path, options, print) => {
    return "TODO BitwiseOrExpression";
};
