import { Doc } from "prettier";
import { printTrailingComments } from "../PrintComments";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ExpressionStatementNode extends SyntaxTreeNode<"ExpressionStatement"> {
    expression: SyntaxTreeNode;
    semicolonToken: SyntaxToken;
}

export const printExpressionStatement: PrintMethod<ExpressionStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [path.call(print, "expression"), ";"];
    printTrailingComments(node, parts, "semicolonToken");
    return concat(parts);
};
