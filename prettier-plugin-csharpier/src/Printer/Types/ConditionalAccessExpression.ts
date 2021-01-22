import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConditionalAccessExpressionNode extends SyntaxTreeNode<"ConditionalAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    whenNotNull: SyntaxTreeNode;
}

export const printConditionalAccessExpression: PrintMethod<ConditionalAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), "?", path.call(print, "whenNotNull")]);
};
