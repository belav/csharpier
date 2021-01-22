import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ContinueStatementNode extends SyntaxTreeNode<"ContinueStatement"> {
    continueKeyword: SyntaxToken;
}

export const printContinueStatement: PrintMethod<ContinueStatementNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "continueKeyword"), ";"]);
};
