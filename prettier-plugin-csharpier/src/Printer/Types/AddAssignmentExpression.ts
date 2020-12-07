import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AddAssignmentExpressionNode extends SyntaxTreeNode<"AddAssignmentExpression"> {}

export const print: PrintMethod<AddAssignmentExpressionNode> = (path, options, print) => {
    return "TODO AddAssignmentExpression";
};
