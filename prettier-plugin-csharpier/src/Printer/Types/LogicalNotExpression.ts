import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LogicalNotExpressionNode extends SyntaxTreeNode<"LogicalNotExpression"> {

}

export const print: PrintMethod<LogicalNotExpressionNode> = (path, options, print) => {
    return "TODO LogicalNotExpression";
};
