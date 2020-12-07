import { PrintMethod } from "../PrintMethod";
import { LeftRightExpression, printLeftRightExpression, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GreaterThanExpressionNode extends SyntaxTreeNode<"GreaterThanExpression">, LeftRightExpression {}

export const print: PrintMethod<GreaterThanExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
