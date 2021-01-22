import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NumericLiteralExpressionNode extends SyntaxTreeNode<"NumericLiteralExpression"> {
    token: SyntaxToken;
}

export const printNumericLiteralExpression: PrintMethod<NumericLiteralExpressionNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "token");
};
