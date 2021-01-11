import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GreaterThanOrEqualExpressionNode extends SyntaxTreeNode<"GreaterThanOrEqualExpression"> {

}

export const print: PrintMethod<GreaterThanOrEqualExpressionNode> = (path, options, print) => {
    return "TODO GreaterThanOrEqualExpression";
};
