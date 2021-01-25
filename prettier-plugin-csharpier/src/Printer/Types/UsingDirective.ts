import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { NameEqualsNode, printNameEquals } from "./NameEquals";

export interface UsingDirectiveNode extends SyntaxTreeNode<"UsingDirective"> {
    usingKeyword: SyntaxToken;
    staticKeyword: SyntaxToken;
    alias?: NameEqualsNode;
    name?: SyntaxTreeNode;
}

export const printUsingDirective: PrintMethod<UsingDirectiveNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push("using ");
    if (node.staticKeyword) {
        parts.push("static ");
    }
    if (node.alias) {
        parts.push(path.call(o => printNameEquals(o, options, print), "alias"));
    }
    parts.push(group(path.call(print, "name")), ";");

    return concat(parts);
};
