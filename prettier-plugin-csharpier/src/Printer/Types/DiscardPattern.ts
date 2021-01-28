import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DiscardPatternNode extends SyntaxTreeNode<"DiscardPattern"> {
    underscoreToken?: SyntaxToken;
}

export const printDiscardPattern: PrintMethod<DiscardPatternNode> = (path, options, print) => {
    return "_";
};
