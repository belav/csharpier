import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RightShiftAssignmentExpressionNode extends SyntaxTreeNode<"RightShiftAssignmentExpression"> {

}

export const print: PrintMethod<RightShiftAssignmentExpressionNode> = (path, options, print) => {
    return "TODO RightShiftAssignmentExpression";
};
