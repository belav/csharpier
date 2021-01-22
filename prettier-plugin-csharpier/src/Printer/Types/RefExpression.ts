import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefExpressionNode extends SyntaxTreeNode<"RefExpression"> {
    refKeyword: SyntaxToken;
    expression: SyntaxTreeNode;
}

export const printRefExpression: PrintMethod<RefExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "refKeyword"), " ", path.call(print, "expression")]);
};
