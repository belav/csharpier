import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NullLiteralExpressionNode extends SyntaxTreeNode<"NullLiteralExpression"> {
    token: SyntaxToken;
}

export const printNullLiteralExpression: PrintMethod<NullLiteralExpressionNode> = (path, options, print) => {
    return "null";
};
