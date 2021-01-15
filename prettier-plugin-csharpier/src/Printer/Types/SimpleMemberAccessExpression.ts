import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode, printPathValue } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { IdentifierNameNode } from "./IdentifierName";

export interface SimpleMemberAccessExpressionNode extends SyntaxTreeNode<"SimpleMemberAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: HasValue;
    name: IdentifierNameNode;
}

export const print: PrintMethod<SimpleMemberAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), printPathValue(path, "operatorToken"), path.call(print, "name")]);
};
