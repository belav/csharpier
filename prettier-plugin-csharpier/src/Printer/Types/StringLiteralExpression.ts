import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StringLiteralExpressionNode extends SyntaxTreeNode<"StringLiteralExpression"> {
    token: SyntaxToken;
}

export const printStringLiteralExpression: PrintMethod<StringLiteralExpressionNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "token");
};
