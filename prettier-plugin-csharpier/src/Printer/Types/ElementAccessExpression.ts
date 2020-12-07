import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ElementAccessExpressionNode extends SyntaxTreeNode<"ElementAccessExpression"> {

}

export const print: PrintMethod<ElementAccessExpressionNode> = (path, options, print) => {
    return "TODO ElementAccessExpression";
};
