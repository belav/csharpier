import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NullLiteralExpressionNode extends SyntaxTreeNode<"NullLiteralExpression"> {

}

export const print: PrintMethod<NullLiteralExpressionNode> = (path, options, print) => {
    return "TODO NullLiteralExpression";
};
