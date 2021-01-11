import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IndexExpressionNode extends SyntaxTreeNode<"IndexExpression"> {

}

export const print: PrintMethod<IndexExpressionNode> = (path, options, print) => {
    return "TODO IndexExpression";
};
