import { PrintMethod } from "../PrintMethod";
import { HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConditionalAccessExpressionNode extends SyntaxTreeNode<"ConditionalAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: HasValue;
    whenNotNull: SyntaxTreeNode;

}

export const print: PrintMethod<ConditionalAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), "?", path.call(print, "whenNotNull")]);
};
