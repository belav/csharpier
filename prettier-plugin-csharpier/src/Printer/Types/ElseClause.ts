import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode } from "./Block";
import { IfStatementNode } from "./IfStatement";

export interface ElseClauseNode extends SyntaxTreeNode<"ElseClause"> {
    elseKeyword: SyntaxToken;
    statement: BlockNode | IfStatementNode;
}

export const printElseClause: PrintMethod<ElseClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "elseKeyword"), " ", path.call(print, "statement")]);
};
