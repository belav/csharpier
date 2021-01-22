import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UsingDirectiveNode extends SyntaxTreeNode<"UsingDirective"> {
    usingKeyword: SyntaxToken;
    staticKeyword: SyntaxToken;
    alias?: SyntaxTreeNode;
}

export const printUsingDirective: PrintMethod<UsingDirectiveNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printSyntaxToken(node.usingKeyword), " ");
    if (node.staticKeyword) {
        parts.push(printSyntaxToken(node.staticKeyword), " ");
    }
    if (node.alias) {
        parts.push(path.call(print, "alias"), " ");
    }
    parts.push(group(path.call(print, "name")), ";");

    return concat(parts);
};
