import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface QueryExpressionNode extends SyntaxTreeNode<"QueryExpression"> {
    fromClause: SyntaxTreeNode;
    body: SyntaxTreeNode;
}

export const printQueryExpression: PrintMethod<QueryExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "fromClause"), " ", path.call(print, "body")]);
};
