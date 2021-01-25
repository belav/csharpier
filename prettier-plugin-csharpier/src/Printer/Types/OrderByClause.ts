import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface OrderByClauseNode extends SyntaxTreeNode<"OrderByClause"> {
    orderByKeyword?: SyntaxToken;
    orderings: OrderingNode[];
}

interface OrderingNode extends SyntaxTreeNode<"Ordering"> {
    expression?: SyntaxTreeNode;
    ascendingOrDescendingKeyword?: SyntaxToken;
}

export const printOrderByClause: PrintMethod<OrderByClauseNode> = (path, options, print) => {
    return concat([
        "orderby ",
        join(
            ", ",
            path.map(orderingPath => {
                const orderingNode = orderingPath.getValue();
                return concat([
                    path.call(print, "expression"),
                    orderingNode.ascendingOrDescendingKeyword
                        ? " " + printSyntaxToken(orderingNode.ascendingOrDescendingKeyword)
                        : "",
                ]);
            }, "orderings"),
        ),
    ]);
};
