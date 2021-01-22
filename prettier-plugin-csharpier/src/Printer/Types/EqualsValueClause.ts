import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EqualsValueClauseNode extends SyntaxTreeNode<"EqualsValueClause"> {
    equalsToken: SyntaxToken;
}

export const printEqualsValueClause: PrintMethod<EqualsValueClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "equalsToken"), " ", path.call(print, "value")]);
};
