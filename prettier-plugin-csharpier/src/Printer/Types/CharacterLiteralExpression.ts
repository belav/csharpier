import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CharacterLiteralExpressionNode extends SyntaxTreeNode<"CharacterLiteralExpression"> {

}

export const print: PrintMethod<CharacterLiteralExpressionNode> = (path, options, print) => {
    return "TODO CharacterLiteralExpression";
};
