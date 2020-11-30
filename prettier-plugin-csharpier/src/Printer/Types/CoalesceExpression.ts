import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CoalesceExpressionNode extends SyntaxTreeNode<"CoalesceExpression"> {

}

export const print: PrintMethod<CoalesceExpressionNode> = (path, options, print) => {
    return "TODO CoalesceExpression";
};
