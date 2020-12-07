import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NumericLiteralExpressionNode extends SyntaxTreeNode<"NumericLiteralExpression"> {

}

export const print: PrintMethod<NumericLiteralExpressionNode> = (path, options, print) => {
    return "TODO NumericLiteralExpression";
};
