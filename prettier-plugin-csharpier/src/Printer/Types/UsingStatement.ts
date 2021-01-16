import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UsingStatementNode extends SyntaxTreeNode<"UsingStatement"> {
    usingKeyword: HasValue;
    declaration: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const print: PrintMethod<UsingStatementNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "usingKeyword"),
        " (",
        path.call(print, "declaration"),
        ")",
        hardline,
        path.call(print, "statement"),
    ]);
};

