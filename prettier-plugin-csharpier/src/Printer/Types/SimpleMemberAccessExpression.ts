import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode, printPathSyntaxToken } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { IdentifierNameNode } from "./IdentifierName";

export interface SimpleMemberAccessExpressionNode extends SyntaxTreeNode<"SimpleMemberAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    name: IdentifierNameNode;
}

export const printSimpleMemberAccessExpression: PrintMethod<SimpleMemberAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), printPathSyntaxToken(path, "operatorToken"), path.call(print, "name")]);
};
