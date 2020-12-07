import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LogicalOrExpressionNode extends SyntaxTreeNode<"LogicalOrExpression"> {

}

export const print: PrintMethod<LogicalOrExpressionNode> = (path, options, print) => {
    return "TODO LogicalOrExpression";
};
