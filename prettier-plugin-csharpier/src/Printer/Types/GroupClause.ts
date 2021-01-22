import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GroupClauseNode extends SyntaxTreeNode<"GroupClause"> {
    groupKeyword: SyntaxToken;
    groupExpression: SyntaxTreeNode;
    byKeyword: SyntaxToken;
    byExpression: SyntaxTreeNode;
}

export const printGroupClause: PrintMethod<GroupClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printSyntaxToken(node.groupKeyword),
        " ",
        path.call(print, "groupExpression"),
        " ",
        printSyntaxToken(node.byKeyword),
        " ",
        path.call(print, "byExpression"),
    ]);
};
