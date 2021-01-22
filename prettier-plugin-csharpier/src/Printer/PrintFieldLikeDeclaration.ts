import { Doc } from "prettier";
import { PrintMethod } from "./PrintMethod";
import { printSyntaxToken, HasModifiers, SyntaxToken, SyntaxTreeNode, printModifiers } from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";

interface FieldLikeDeclarationNode extends SyntaxTreeNode<"FieldDeclaration" | "EventFieldDeclaration">, HasModifiers {
    attributeLists: SyntaxTreeNode[];
    eventKeyword?: SyntaxToken;
    declaration: SyntaxTreeNode;
}

export const printFieldLikeDeclaration: PrintMethod<FieldLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    if (node.eventKeyword) {
        parts.push(printSyntaxToken(node.eventKeyword), " ");
    }

    parts.push(path.call(print, "declaration"));
    parts.push(";");
    return concat(parts);
};
