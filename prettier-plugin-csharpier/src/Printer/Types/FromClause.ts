import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printIdentifier, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FromClauseNode extends SyntaxTreeNode<"FromClause"> {
    fromKeyword: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier: SyntaxToken;
    inKeyword: SyntaxToken;
    expression: SyntaxTreeNode;
}

export const printFromClause: PrintMethod<FromClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printSyntaxToken(node.fromKeyword),
        " ",
        printIdentifier(node),
        " ",
        printSyntaxToken(node.inKeyword),
        " ",
        path.call(print, "expression"),
    ]);
};
