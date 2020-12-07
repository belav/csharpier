import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NotEqualsExpressionNode extends SyntaxTreeNode<"NotEqualsExpression"> {

}

export const print: PrintMethod<NotEqualsExpressionNode> = (path, options, print) => {
    return "TODO NotEqualsExpression";
};
