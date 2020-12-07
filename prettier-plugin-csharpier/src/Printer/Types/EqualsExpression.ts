import { PrintMethod } from "../PrintMethod";
import { LeftRightExpression, HasValue, printLeftRightExpression, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EqualsExpressionNode extends SyntaxTreeNode<"EqualsExpression">, LeftRightExpression {}

export const print: PrintMethod<EqualsExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
