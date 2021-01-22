import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArgumentNode extends SyntaxTreeNode<"Argument"> {
    expression: SyntaxTreeNode;
    refKindKeyword: SyntaxToken;
}

export const printArgument: PrintMethod<ArgumentNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.refKindKeyword?.text) {
        parts.push(printSyntaxToken(node.refKindKeyword), " ");
    }
    parts.push(path.call(print, "expression"));

    return concat(parts);
};
