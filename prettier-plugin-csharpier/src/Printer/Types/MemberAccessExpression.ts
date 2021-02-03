import { PrintMethod } from "../PrintMethod";
import { printPathSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { IdentifierNameNode } from "./IdentifierName";

export interface MemberAccessExpressionNode extends SyntaxTreeNode<"MemberAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    name: IdentifierNameNode;
}

export const printMemberAccessExpression: PrintMethod<MemberAccessExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "expression"),
        printPathSyntaxToken(path, "operatorToken"),
        path.call(print, "name")
    ]);
};
