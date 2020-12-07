import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArgumentNode extends SyntaxTreeNode<"Argument"> {
    expression: SyntaxTreeNode;
    refKindKeyword: HasValue | null;
}

export const print: PrintMethod<ArgumentNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.refKindKeyword) {
        parts.push(printValue(node.refKindKeyword), " ");
    }
    parts.push(path.call(print, "expression"));

    return concat(parts);
};
