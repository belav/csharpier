import { PrintMethod } from "../PrintMethod";
import { LeftRightExpression, printLeftRightExpression, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LogicalOrExpressionNode extends SyntaxTreeNode<"LogicalOrExpression">, LeftRightExpression {}

export const print: PrintMethod<LogicalOrExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
