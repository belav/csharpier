import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode, LeftRightExpression, printLeftRightExpression } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SimpleAssignmentExpressionNode
    extends SyntaxTreeNode<"SimpleAssignmentExpression">,
        LeftRightExpression {}

export const print: PrintMethod<SimpleAssignmentExpressionNode> = (path, options, print) => {
    return printLeftRightExpression(path, print);
};
