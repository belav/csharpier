import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForStatementNode extends SyntaxTreeNode<"ForStatement"> {
    forKeyword: SyntaxToken;
    declaration: SyntaxTreeNode;
    initializers: SyntaxTreeNode[];
    condition: SyntaxTreeNode;
    incrementors: SyntaxTreeNode[];
    statement: SyntaxTreeNode;
}

export const printForStatement: PrintMethod<ForStatementNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "forKeyword"),
        " (",
        path.call(print, "declaration"),
        "; ",
        path.call(print, "condition"),
        "; ",
        join(", ", path.map(print, "incrementors")),
        ")",
        path.call(print, "statement"),
    ]);
};
