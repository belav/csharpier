import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StringLiteralExpressionNode extends SyntaxTreeNode<"StringLiteralExpression"> {

}

export const print: PrintMethod<StringLiteralExpressionNode> = (path, options, print) => {
    return "TODO StringLiteralExpression";
};
