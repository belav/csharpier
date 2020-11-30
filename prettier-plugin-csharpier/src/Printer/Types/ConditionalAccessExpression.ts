import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConditionalAccessExpressionNode extends SyntaxTreeNode<"ConditionalAccessExpression"> {

}

export const print: PrintMethod<ConditionalAccessExpressionNode> = (path, options, print) => {
    return "TODO ConditionalAccessExpression";
};
