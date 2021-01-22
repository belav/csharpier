import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode, printPathSyntaxToken } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrowExpressionClauseNode extends SyntaxTreeNode<"ArrowExpressionClause"> {
    arrowToken: SyntaxToken;
    expression: SyntaxTreeNode;
}

export const printArrowExpressionClause: PrintMethod<ArrowExpressionClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "arrowToken"), " ", path.call(print, "expression")]);
};
