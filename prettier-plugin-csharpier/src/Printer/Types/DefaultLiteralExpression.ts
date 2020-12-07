import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DefaultLiteralExpressionNode extends SyntaxTreeNode<"DefaultLiteralExpression"> {

}

export const print: PrintMethod<DefaultLiteralExpressionNode> = (path, options, print) => {
    return "TODO DefaultLiteralExpression";
};
