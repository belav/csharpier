import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UsingStatementNode extends SyntaxTreeNode<"UsingStatement"> {
    usingKeyword: SyntaxToken;
    declaration: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const printUsingStatement: PrintMethod<UsingStatementNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "usingKeyword"),
        " (",
        path.call(print, "declaration"),
        ")",
        hardline,
        path.call(print, "statement"),
    ]);
};

