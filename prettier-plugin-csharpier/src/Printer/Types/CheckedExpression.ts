import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CheckedExpressionNode extends SyntaxTreeNode<"CheckedExpression"> {
    keyword: SyntaxToken;
    expression: SyntaxTreeNode;
}

export const printCheckedExpression: PrintMethod<CheckedExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "keyword"), "(", path.call(print, "expression"), ")"]);
};
