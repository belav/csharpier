import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TrueLiteralExpressionNode extends SyntaxTreeNode<"TrueLiteralExpression"> {}

export const printTrueLiteralExpression: PrintMethod<TrueLiteralExpressionNode> = (path, options, print) => {
    return "true";
};
