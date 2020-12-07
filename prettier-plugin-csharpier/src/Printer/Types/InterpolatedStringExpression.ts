import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolatedStringExpressionNode extends SyntaxTreeNode<"InterpolatedStringExpression"> {

}

export const print: PrintMethod<InterpolatedStringExpressionNode> = (path, options, print) => {
    return "TODO InterpolatedStringExpression";
};
