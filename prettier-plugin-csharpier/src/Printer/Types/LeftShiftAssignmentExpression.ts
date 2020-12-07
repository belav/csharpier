import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LeftShiftAssignmentExpressionNode extends SyntaxTreeNode<"LeftShiftAssignmentExpression"> {}

export const print: PrintMethod<LeftShiftAssignmentExpressionNode> = (path, options, print) => {
    return "TODO LeftShiftAssignmentExpression";
};
