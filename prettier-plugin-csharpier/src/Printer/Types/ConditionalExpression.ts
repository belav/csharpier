import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConditionalExpressionNode extends SyntaxTreeNode<"ConditionalExpression"> {

}

export const print: PrintMethod<ConditionalExpressionNode> = (path, options, print) => {
    return "TODO ConditionalExpression";
};
