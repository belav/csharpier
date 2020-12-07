import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NullLiteralExpressionNode extends SyntaxTreeNode<"NullLiteralExpression"> {
    token: HasValue;
}

export const print: PrintMethod<NullLiteralExpressionNode> = (path, options, print) => {
    return "null";
};
