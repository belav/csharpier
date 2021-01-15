import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NumericLiteralExpressionNode extends SyntaxTreeNode<"NumericLiteralExpression"> {
    token: HasValue;
}

export const print: PrintMethod<NumericLiteralExpressionNode> = (path, options, print) => {
    return printPathValue(path, "token");
};
