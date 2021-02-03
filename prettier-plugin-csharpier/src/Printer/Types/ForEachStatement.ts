import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printIdentifier, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForEachStatementNode extends SyntaxTreeNode<"ForEachStatement"> {
    awaitKeyword?: SyntaxToken;
    forEachKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier: SyntaxToken;
    inKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

export const printForEachStatement: PrintMethod<ForEachStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.awaitKeyword) {
        parts.push("await ");
    }
    parts.push("foreach ", "(");
    parts.push(path.call(print, "type"), " ", printIdentifier(node), " in ", path.call(print, "expression"));
    parts.push(")");
    parts.push(path.call(print, "statement"));

    return concat(parts);
};
