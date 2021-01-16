import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface WhileStatementNode extends SyntaxTreeNode<"WhileStatement"> {
    whileKeyword: HasValue;
    condition: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const print: PrintMethod<WhileStatementNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "whileKeyword"),
        " (",
        path.call(print, "condition"),
        ")",
        path.call(print, "statement"),
    ]);
};
