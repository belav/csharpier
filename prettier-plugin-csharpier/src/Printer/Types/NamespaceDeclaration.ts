import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { printExtraNewLines } from "../PrintExtraNewLines";
import { PrintMethod } from "../PrintMethod";
import { printModifiers, printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { ExternAliasDirectiveNode, printExternAliasDirective } from "./ExternAliasDirective";
import { printUsingDirective, UsingDirectiveNode } from "./UsingDirective";
import has = Reflect.has;

export interface NamespaceDeclarationNode extends SyntaxTreeNode<"NamespaceDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    namespaceKeyword?: SyntaxToken;
    name?: SyntaxTreeNode;
    externs: ExternAliasDirectiveNode[];
    usings: UsingDirectiveNode[];
    members: SyntaxTreeNode[];
}

export const printNamespaceDeclaration: PrintMethod<NamespaceDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printExtraNewLines(node, parts, "attributeLists", "modifiers", "namespaceKeyword");
    printAttributeLists(node, parts, path, options, print);
    parts.push(printModifiers(node));
    parts.push(printSyntaxToken(node.namespaceKeyword));
    parts.push(" ");
    parts.push(path.call(print, "name"));

    const hasMembers = node.members.length > 0;
    const hasUsing = node.usings.length > 0;
    const hasExterns = node.externs.length > 0;
    if (hasMembers || hasUsing || hasExterns) {
        parts.push(concat([hardline, "{"]));

        const innerParts: Doc[] = [];
        innerParts.push(hardline);
        if (hasExterns) {
            innerParts.push(
                join(
                    hardline,
                    path.map(innerPath => printExternAliasDirective(innerPath, options, print), "externs"),
                ),
                hardline,
            );
        }

        if (hasUsing) {
            innerParts.push(
                join(
                    hardline,
                    path.map(innerPath => printUsingDirective(innerPath, options, print), "usings"),
                ),
                hardline,
            );
        }

        if (hasMembers) {
            innerParts.push(join(hardline, path.map(print, "members")), hardline);
        }

        innerParts.splice(innerParts.length - 1, 1);

        parts.push(indent(concat(innerParts)));
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", "}");
    }

    return concat([concat(parts)]);
};
