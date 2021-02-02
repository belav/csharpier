import { Doc } from "prettier";
import { printAttributeLists } from "./PrintAttributeLists";
import { printLeadingComments, printTrailingComments } from "./PrintComments";
import { printExtraNewLines } from "./PrintExtraNewLines";
import { PrintMethod } from "./PrintMethod";
import { printSyntaxToken, HasModifiers, SyntaxToken, SyntaxTreeNode, printModifiers } from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";
import { AttributeListNode } from "./Types/AttributeList";
import { VariableDeclarationNode } from "./Types/VariableDeclaration";

interface FieldLikeDeclarationNode extends SyntaxTreeNode<"FieldDeclaration" | "EventFieldDeclaration">, HasModifiers {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    eventKeyword?: SyntaxToken;
    declaration: VariableDeclarationNode;
    semicolonToken?: SyntaxToken;
}

export const printFieldLikeDeclaration: PrintMethod<FieldLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printExtraNewLines(node, parts, "attributeLists", "modifiers", "eventKeyword", "declaration");
    printAttributeLists(node, parts, path, options, print);
    printLeadingComments(node, parts, "modifiers", "eventKeyword", "declaration");
    parts.push(printModifiers(node));
    if (node.eventKeyword) {
        parts.push(printSyntaxToken(node.eventKeyword), " ");
    }

    parts.push(path.call(print, "declaration"));
    parts.push(";");
    printTrailingComments(node, parts, "semicolonToken");
    return concat(parts);
};
