import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LogicalAndExpressionNode extends SyntaxTreeNode<"LogicalAndExpression"> {

}

export const print: PrintMethod<LogicalAndExpressionNode> = (path, options, print) => {
    return "TODO LogicalAndExpression";
};
