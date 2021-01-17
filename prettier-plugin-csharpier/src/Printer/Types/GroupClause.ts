import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GroupClauseNode extends SyntaxTreeNode<"GroupClause"> {
    groupKeyword: HasValue;
    groupExpression: SyntaxTreeNode;
    byKeyword: HasValue;
    byExpression: SyntaxTreeNode;
}

export const print: PrintMethod<GroupClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printValue(node.groupKeyword),
        " ",
        path.call(print, "groupExpression"),
        " ",
        printValue(node.byKeyword),
        " ",
        path.call(print, "byExpression"),
    ]);
};
