import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DefaultLiteralExpressionNode extends SyntaxTreeNode<"DefaultLiteralExpression"> {
    token: SyntaxToken;
}

export const printDefaultLiteralExpression: PrintMethod<DefaultLiteralExpressionNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "token");
};
