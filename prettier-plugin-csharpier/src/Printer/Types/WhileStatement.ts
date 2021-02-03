import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface WhileStatementNode extends SyntaxTreeNode<"WhileStatement"> {
    whileKeyword: SyntaxToken;
    condition: SyntaxTreeNode;
    statement: SyntaxTreeNode;
}

export const printWhileStatement: PrintMethod<WhileStatementNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "whileKeyword"),
        " (",
        path.call(print, "condition"),
        ")",
        path.call(print, "statement")
    ]);
};
