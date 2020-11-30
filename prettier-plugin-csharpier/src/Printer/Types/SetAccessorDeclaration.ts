import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SetAccessorDeclarationNode extends SyntaxTreeNode<"SetAccessorDeclaration"> {
    keyword: HasValue;
    body: unknown;
}

export const print: PrintMethod<SetAccessorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(getValue(node.keyword));
    if (!node.body) {
        parts.unshift(line);
        parts.push(";")
    } else {
        parts.unshift(hardline);
        parts.push(path.call(print, "body"));
    }

    return concat(parts);
};
