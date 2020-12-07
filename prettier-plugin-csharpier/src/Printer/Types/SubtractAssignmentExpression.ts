import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SubtractAssignmentExpressionNode extends SyntaxTreeNode<"SubtractAssignmentExpression"> {

}

export const print: PrintMethod<SubtractAssignmentExpressionNode> = (path, options, print) => {
    return "TODO SubtractAssignmentExpression";
};
