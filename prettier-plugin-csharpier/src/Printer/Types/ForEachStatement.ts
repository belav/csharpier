import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printIdentifier, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForEachStatementNode extends SyntaxTreeNode<"ForEachStatement">, HasIdentifier {
    forEachKeyword: SyntaxToken;
    inKeyword: SyntaxToken;
}

export const printForEachStatement: PrintMethod<ForEachStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printSyntaxToken(node.forEachKeyword), " ", "(");
    parts.push(
        path.call(print, "type"),
        " ",
        printIdentifier(node),
        " ",
        printSyntaxToken(node.inKeyword),
        " ",
        path.call(print, "expression"),
    );
    parts.push(")");
    parts.push(path.call(print, "statement"));

    return concat(parts);
};
