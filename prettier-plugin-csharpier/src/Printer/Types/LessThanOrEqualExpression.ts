import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LessThanOrEqualExpressionNode extends SyntaxTreeNode<"LessThanOrEqualExpression"> {

}

export const print: PrintMethod<LessThanOrEqualExpressionNode> = (path, options, print) => {
    return "TODO LessThanOrEqualExpression";
};
