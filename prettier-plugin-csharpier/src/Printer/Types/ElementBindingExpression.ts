import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ElementBindingExpressionNode extends SyntaxTreeNode<"ElementBindingExpression"> {
    argumentList: SyntaxTreeNode;
}

export const print: PrintMethod<ElementBindingExpressionNode> = (path, options, print) => {
    return path.call(print, "argumentList");
};
