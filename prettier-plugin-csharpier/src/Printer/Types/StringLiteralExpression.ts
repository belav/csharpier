import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StringLiteralExpressionNode extends SyntaxTreeNode<"StringLiteralExpression"> {
    token: HasValue;
}

export const print: PrintMethod<StringLiteralExpressionNode> = (path, options, print) => {
    return printValue(path.getValue().token);
};
