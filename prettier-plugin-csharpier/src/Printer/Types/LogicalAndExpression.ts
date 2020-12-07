import { PrintMethod } from "../PrintMethod";
import { LeftRightExpression, printLeftRightExpression, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LogicalAndExpressionNode extends SyntaxTreeNode<"LogicalAndExpression">, LeftRightExpression {}

export const print: PrintMethod<LogicalAndExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
