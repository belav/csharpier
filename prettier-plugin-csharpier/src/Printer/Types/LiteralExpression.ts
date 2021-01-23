import { PrintMethod } from "../PrintMethod";
import { printPathSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";

export interface LiteralExpressionNode extends SyntaxTreeNode<"LiteralExpression"> {
    token: SyntaxToken;
}

export const printLiteralExpression: PrintMethod<LiteralExpressionNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "token");
};
