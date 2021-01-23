import { Doc } from "prettier";
import { printAttributeLists } from "./PrintAttributeLists";
import { PrintMethod } from "./PrintMethod";
import { printSyntaxToken, HasModifiers, SyntaxToken, SyntaxTreeNode, printModifiers } from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";
import { AttributeListNode } from "./Types/AttributeList";
import { VariableDeclarationNode } from "./Types/VariableDeclaration";

interface FieldLikeDeclarationNode extends SyntaxTreeNode<"FieldDeclaration" | "EventFieldDeclaration">, HasModifiers {
    attributeLists: AttributeListNode[];
    eventKeyword?: SyntaxToken;
    declaration: VariableDeclarationNode;
}

export const printFieldLikeDeclaration: PrintMethod<FieldLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printAttributeLists(node, parts, path, options, print);
    parts.push(printModifiers(node));
    if (node.eventKeyword) {
        parts.push(printSyntaxToken(node.eventKeyword), " ");
    }

    parts.push(path.call(print, "declaration"));
    parts.push(";");
    return concat(parts);
};
