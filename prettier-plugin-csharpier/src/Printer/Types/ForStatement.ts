import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForStatementNode extends SyntaxTreeNode<"ForStatement"> {
    forKeyword: HasValue;
    declaration: SyntaxTreeNode;
    initializers: SyntaxTreeNode[];
    condition: SyntaxTreeNode;
    incrementors: SyntaxTreeNode[];
    statement: SyntaxTreeNode;
}

export const print: PrintMethod<ForStatementNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "forKeyword"),
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
