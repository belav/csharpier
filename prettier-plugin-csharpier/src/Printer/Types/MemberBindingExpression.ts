import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { IdentifierNameNode } from "./IdentifierName";

export interface MemberBindingExpressionNode extends SyntaxTreeNode<"MemberBindingExpression"> {
    operatorToken: SyntaxToken;
    name: IdentifierNameNode;
}

export const printMemberBindingExpression: PrintMethod<MemberBindingExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "operatorToken"), path.call(print, "name")]);
};
