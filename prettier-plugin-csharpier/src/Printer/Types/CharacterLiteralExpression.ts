import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CharacterLiteralExpressionNode extends SyntaxTreeNode<"CharacterLiteralExpression"> {
    token: HasValue;
}

export const print: PrintMethod<CharacterLiteralExpressionNode> = (path, options, print) => {
    return printPathValue(path, "token");
};
