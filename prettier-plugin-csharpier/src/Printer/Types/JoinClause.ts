import { PrintMethod } from "../PrintMethod";
import { printIdentifier, printPathIdentifier, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface JoinClauseNode extends SyntaxTreeNode<"JoinClause"> {
    joinKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier: SyntaxToken;
    inKeyword?: SyntaxToken;
    inExpression?: SyntaxTreeNode;
    onKeyword?: SyntaxToken;
    leftExpression?: SyntaxTreeNode;
    equalsKeyword?: SyntaxToken;
    rightExpression?: SyntaxTreeNode;
    into?: JoinIntoClauseNode;
}

interface JoinIntoClauseNode extends SyntaxTreeNode<"JoinIntoClause"> {
    intoKeyword?: SyntaxToken;
    identifier: SyntaxToken;
}

export const printJoinClause: PrintMethod<JoinClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        "join ",
        printIdentifier(node),
        " in ",
        path.call(print, "inExpression"),
        " on ",
        path.call(print, "leftExpression"),
        " equals ",
        path.call(print, "rightExpression"),
        node.into ? " into " + printIdentifier(node.into) : "",
    ]);
};
