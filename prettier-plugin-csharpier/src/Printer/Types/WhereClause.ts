import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface WhereClauseNode extends SyntaxTreeNode<"WhereClause"> {
    whereKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
}

export const printWhereClause: PrintMethod<WhereClauseNode> = (path, options, print) => {
    return concat(["where ", path.call(print, "condition")]);
};
