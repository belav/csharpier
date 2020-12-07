import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AndAssignmentExpressionNode extends SyntaxTreeNode<"AndAssignmentExpression"> {}

export const print: PrintMethod<AndAssignmentExpressionNode> = (path, options, print) => {
    return "TODO AndAssignmentExpression";
};
