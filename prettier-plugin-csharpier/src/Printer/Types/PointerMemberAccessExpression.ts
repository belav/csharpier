import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PointerMemberAccessExpressionNode extends SyntaxTreeNode<"PointerMemberAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    name: SyntaxTreeNode;
}

export const printPointerMemberAccessExpression: PrintMethod<PointerMemberAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), printPathSyntaxToken(path, "operatorToken"), path.call(print, "name")]);
};
