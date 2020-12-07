import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LeftShiftExpressionNode extends SyntaxTreeNode<"LeftShiftExpression"> {}

export const print: PrintMethod<LeftShiftExpressionNode> = (path, options, print) => {
    return "TODO LeftShiftExpression";
};
