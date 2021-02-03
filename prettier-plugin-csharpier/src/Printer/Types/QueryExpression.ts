import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { FromClauseNode, printFromClause } from "./FromClause";
import { printQueryBody, QueryBodyNode } from "./QueryBody";

export interface QueryExpressionNode extends SyntaxTreeNode<"QueryExpression"> {
    fromClause: FromClauseNode;
    body: QueryBodyNode;
}

export const printQueryExpression: PrintMethod<QueryExpressionNode> = (path, options, print) => {
    return concat([
        path.call(o => printFromClause(o, options, print), "fromClause"),
        indent(concat([line, path.call(o => printQueryBody(o, options, print), "body")]))
    ]);
};
