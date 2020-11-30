import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SimpleMemberAccessExpressionNode extends SyntaxTreeNode<"SimpleMemberAccessExpression"> {
    statements: SyntaxTreeNode[];
}

export const print: PrintMethod<SimpleMemberAccessExpressionNode> = (path, options, print) => {
    throw "TODO SimpleMemberAccessExpression";
};
