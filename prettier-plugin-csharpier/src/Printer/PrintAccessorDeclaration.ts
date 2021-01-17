import { Doc } from "prettier";
import { PrintMethod } from "./PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";

interface AccessorDeclarationNode
    extends SyntaxTreeNode<
        "SetAccessorDeclaration" | "GetAccessorDeclaration" | "AddAccessorDeclaration" | "RemoveAccessorDeclaration"
    > {
    keyword: HasValue;
    expressionBody?: SyntaxTreeNode;
    body?: SyntaxTreeNode;
}

export const print: PrintMethod<AccessorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printValue(node.keyword));
    if (!node.body && !node.expressionBody) {
        parts.unshift(line);
        parts.push(";");
    } else {
        parts.unshift(hardline);
        if (node.body) {
            parts.push(path.call(print, "body"));
        } else {
            parts.push(" ");
            parts.push(path.call(print, "expressionBody"));
            parts.push(";");
        }
    }

    return concat(parts);
};
