import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DoStatementNode extends SyntaxTreeNode<"DoStatement"> {
    doKeyword: HasValue;
    statement: SyntaxTreeNode;
    whileKeyword: HasValue;
    condition: SyntaxTreeNode;
}

export const print: PrintMethod<DoStatementNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "doKeyword"),
        path.call(print, "statement"),
        hardline,
        printPathValue(path, "whileKeyword"),
        " (",
        path.call(print, "condition"),
        ");",
    ]);
};
