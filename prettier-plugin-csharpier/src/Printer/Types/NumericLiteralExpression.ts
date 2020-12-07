import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NumericLiteralExpressionNode extends SyntaxTreeNode<"NumericLiteralExpression"> {
    token: HasValue
}

export const print: PrintMethod<NumericLiteralExpressionNode> = (path, options, print) => {
    return printValue(path.getValue().token);
};
