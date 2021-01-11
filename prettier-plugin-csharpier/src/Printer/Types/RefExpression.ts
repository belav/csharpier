import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefExpressionNode extends SyntaxTreeNode<"RefExpression"> {

}

export const print: PrintMethod<RefExpressionNode> = (path, options, print) => {
    return "TODO RefExpression";
};
