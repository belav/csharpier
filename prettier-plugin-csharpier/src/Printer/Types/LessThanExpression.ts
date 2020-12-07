import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LessThanExpressionNode extends SyntaxTreeNode<"LessThanExpression"> {

}

export const print: PrintMethod<LessThanExpressionNode> = (path, options, print) => {
    return "TODO LessThanExpression";
};
