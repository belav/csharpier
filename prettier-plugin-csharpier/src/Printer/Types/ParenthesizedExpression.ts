import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ParenthesizedExpressionNode extends SyntaxTreeNode<"ParenthesizedExpression"> {

}

export const print: PrintMethod<ParenthesizedExpressionNode> = (path, options, print) => {
    return "TODO ParenthesizedExpression";
};
