import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DefaultLiteralExpressionNode extends SyntaxTreeNode<"DefaultLiteralExpression"> {
    token: HasValue;
}

export const print: PrintMethod<DefaultLiteralExpressionNode> = (path, options, print) => {
    return printPathValue(path, "token");
};
