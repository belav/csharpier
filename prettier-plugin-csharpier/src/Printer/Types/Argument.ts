import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { NameColonNode, printNameColon } from "./NameColon";

export interface ArgumentNode extends SyntaxTreeNode<"Argument"> {
    nameColon?: NameColonNode;
    refKindKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    refOrOutKeyword?: SyntaxToken;
}

export const printArgument: PrintMethod<ArgumentNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.nameColon) {
        parts.push(path.call(o => printNameColon(o, options, print), "nameColon"));
    }
    if (node.refKindKeyword) {
        parts.push(printSyntaxToken(node.refKindKeyword), " ");
    }
    parts.push(path.call(print, "expression"));

    return concat(parts);
};
