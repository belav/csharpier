import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CharacterLiteralExpressionNode extends SyntaxTreeNode<"CharacterLiteralExpression"> {
    token: SyntaxToken;
}

export const printCharacterLiteralExpression: PrintMethod<CharacterLiteralExpressionNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "token");
};
