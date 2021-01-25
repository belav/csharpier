import { PrintMethod } from "../PrintMethod";
import { printIdentifier, printPathIdentifier, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LetClauseNode extends SyntaxTreeNode<"LetClause"> {
    letKeyword?: SyntaxToken;
    identifier: SyntaxToken;
    equalsToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

export const printLetClause: PrintMethod<LetClauseNode> = (path, options, print) => {
    return concat(["let ", printPathIdentifier(path), " = ", path.call(print, "expression")]);
};
