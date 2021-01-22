import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode, printPathSyntaxToken } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThisExpressionNode extends SyntaxTreeNode<"ThisExpression"> {
    token: SyntaxToken;
}

export const printThisExpression: PrintMethod<ThisExpressionNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "token");
};
