import { PrintMethod } from "../PrintMethod";
import { LeftRightExpression, printLeftRightExpression, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LessThanOrEqualExpressionNode
    extends SyntaxTreeNode<"LessThanOrEqualExpression">,
        LeftRightExpression {}

export const print: PrintMethod<LessThanOrEqualExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
