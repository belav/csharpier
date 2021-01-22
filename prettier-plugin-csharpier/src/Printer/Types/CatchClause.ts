import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CatchClauseNode extends SyntaxTreeNode<"CatchClause"> {
    catchKeyword: SyntaxToken;
    declaration: SyntaxTreeNode;
    block: SyntaxTreeNode;
}

export const printCatchClause: PrintMethod<CatchClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "catchKeyword"), " ", path.call(print, "declaration"), path.call(print, "block")]);
};
