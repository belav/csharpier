import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GreaterThanExpressionNode extends SyntaxTreeNode<"GreaterThanExpression"> {

}

export const print: PrintMethod<GreaterThanExpressionNode> = (path, options, print) => {
    return "TODO GreaterThanExpression";
};
