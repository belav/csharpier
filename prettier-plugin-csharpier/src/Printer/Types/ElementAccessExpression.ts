import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ElementAccessExpressionNode extends SyntaxTreeNode<"ElementAccessExpression"> {
    expression: SyntaxTreeNode;
    argumentList: SyntaxTreeNode;
}

export const printElementAccessExpression: PrintMethod<ElementAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), path.call(print, "argumentList")]);
};
