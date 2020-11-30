import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { IdentifierNameNode } from "./IdentifierName";

export interface SimpleMemberAccessExpressionNode extends SyntaxTreeNode<"SimpleMemberAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: HasValue;
    name: IdentifierNameNode;
}

export const print: PrintMethod<SimpleMemberAccessExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([path.call(print, "expression"), printValue(node.operatorToken), path.call(print, "name")]);
};
