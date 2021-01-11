import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CoalesceAssignmentExpressionNode extends SyntaxTreeNode<"CoalesceAssignmentExpression"> {

}

export const print: PrintMethod<CoalesceAssignmentExpressionNode> = (path, options, print) => {
    return "TODO CoalesceAssignmentExpression";
};
