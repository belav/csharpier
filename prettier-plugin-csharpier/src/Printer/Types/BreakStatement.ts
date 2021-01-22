import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BreakStatementNode extends SyntaxTreeNode<"BreakStatement"> {
    breakKeyword: SyntaxToken;
}

export const printBreakStatement: PrintMethod<BreakStatementNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "breakKeyword"), ";"]);
};
