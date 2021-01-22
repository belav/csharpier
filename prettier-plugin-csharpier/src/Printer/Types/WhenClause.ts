import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface WhenClauseNode extends SyntaxTreeNode<"WhenClause"> {
    whenKeyword: SyntaxToken;
    condition: SyntaxTreeNode;
}

export const printWhenClause: PrintMethod<WhenClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "whenKeyword"), " ", path.call(print, "condition")]);
};
