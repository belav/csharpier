import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, HasValue, printIdentifier, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FromClauseNode extends SyntaxTreeNode<"FromClause"> {
    fromKeyword: HasValue;
    type?: SyntaxTreeNode;
    identifier: HasValue;
    inKeyword: HasValue;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<FromClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printValue(node.fromKeyword),
        " ",
        printIdentifier(node),
        " ",
        printValue(node.inKeyword),
        " ",
        path.call(print, "expression"),
    ]);
};
