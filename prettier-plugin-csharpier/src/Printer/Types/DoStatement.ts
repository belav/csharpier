import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DoStatementNode extends SyntaxTreeNode<"DoStatement"> {
    doKeyword: SyntaxToken;
    statement: SyntaxTreeNode;
    whileKeyword: SyntaxToken;
    condition: SyntaxTreeNode;
}

export const printDoStatement: PrintMethod<DoStatementNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "doKeyword"),
        path.call(print, "statement"),
        hardline,
        printPathSyntaxToken(path, "whileKeyword"),
        " (",
        path.call(print, "condition"),
        ");",
    ]);
};
