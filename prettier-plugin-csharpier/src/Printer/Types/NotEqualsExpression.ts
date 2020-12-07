import { PrintMethod } from "../PrintMethod";
import { LeftRightExpression, printLeftRightExpression, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NotEqualsExpressionNode extends SyntaxTreeNode<"NotEqualsExpression">, LeftRightExpression {

}

export const print: PrintMethod<NotEqualsExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
