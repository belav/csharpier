import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BaseExpressionNode extends SyntaxTreeNode<"BaseExpression"> {

}

export const print: PrintMethod<BaseExpressionNode> = (path, options, print) => {
    return "TODO BaseExpression";
};
