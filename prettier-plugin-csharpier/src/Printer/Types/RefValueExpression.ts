import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefValueExpressionNode extends SyntaxTreeNode<"RefValueExpression"> {

}

export const print: PrintMethod<RefValueExpressionNode> = (path, options, print) => {
    return "TODO RefValueExpression";
};
