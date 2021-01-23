import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

interface AccessorDeclarationNode
    extends SyntaxTreeNode<
        "SetAccessorDeclaration" | "GetAccessorDeclaration" | "AddAccessorDeclaration" | "RemoveAccessorDeclaration"
        > {
    keyword: SyntaxToken;
    expressionBody?: SyntaxTreeNode;
    body?: SyntaxTreeNode;
}

export const printAccessorDeclaration: PrintMethod<AccessorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printSyntaxToken(node.keyword));
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
