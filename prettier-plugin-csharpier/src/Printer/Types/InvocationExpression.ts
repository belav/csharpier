import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InvocationExpressionNode extends SyntaxTreeNode<"InvocationExpression"> {
    expression: SyntaxTreeNode;
    argumentList: SyntaxTreeNode;
}

export const print: PrintMethod<InvocationExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), path.call(print, "argumentList")]);
};
