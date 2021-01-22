import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AwaitExpressionNode extends SyntaxTreeNode<"AwaitExpression"> {
    awaitKeyword: SyntaxToken;
    expression: SyntaxTreeNode;
}

export const printAwaitExpression: PrintMethod<AwaitExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "awaitKeyword"), " ", path.call(print, "expression")]);
};
