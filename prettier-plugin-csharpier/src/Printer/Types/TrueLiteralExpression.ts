import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TrueLiteralExpressionNode extends SyntaxTreeNode<"TrueLiteralExpression"> {}

export const print: PrintMethod<TrueLiteralExpressionNode> = (path, options, print) => {
    return "true";
};
