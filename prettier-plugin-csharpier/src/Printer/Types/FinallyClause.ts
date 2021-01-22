import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FinallyClauseNode extends SyntaxTreeNode<"FinallyClause"> {
    finallyKeyword: SyntaxToken;
    block: SyntaxTreeNode;
}

export const printFinallyClause: PrintMethod<FinallyClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "finallyKeyword"), path.call(print, "block")]);
};
