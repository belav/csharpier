import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FixedStatementNode extends SyntaxTreeNode<"FixedStatement"> {
    fixedKeyword: SyntaxToken;
    declaration: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const printFixedStatement: PrintMethod<FixedStatementNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "fixedKeyword"),
        " ",
        "(",
        path.call(print, "declaration"),
        ")",
        path.call(print, "statement")
    ]);
};
