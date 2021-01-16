import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FixedStatementNode extends SyntaxTreeNode<"FixedStatement"> {
    fixedKeyword: HasValue;
    declaration: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const print: PrintMethod<FixedStatementNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "fixedKeyword"),
        " ",
        "(",
        path.call(print, "declaration"),
        ")",
        path.call(print, "statement"),
    ]);
};
