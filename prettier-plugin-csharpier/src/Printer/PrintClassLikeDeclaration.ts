import { Doc } from "prettier";
import { concat, hardline, indent, join } from "./Builders";
import { printComments } from "./Comments";
import { PrintMethod } from "./PrintMethod";
import {
    HasIdentifier,
    HasModifiers,
    SyntaxToken,
    printIdentifier,
    printModifiers,
    printSyntaxToken,
    SyntaxTreeNode
} from "./SyntaxTreeNode";
import { BaseListNode } from "./Types/BaseList";

export interface ClassLikeDeclarationNode extends SyntaxTreeNode<"ClassDeclaration" | "EnumDeclaration" | "StructDeclaration">, HasModifiers, HasIdentifier {
    keyword?: SyntaxToken;
    enumKeyword?: SyntaxToken;
    typeParameterList?: SyntaxTreeNode;
    baseList: BaseListNode;
    members: SyntaxTreeNode[];
}

export const printClassLikeDeclaration: PrintMethod<ClassLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printComments(parts, node);
    parts.push(printModifiers(node));
    if (node.keyword) {
        parts.push(printSyntaxToken(node.keyword));
    }
    if (node.enumKeyword) {
        parts.push(printSyntaxToken(node.enumKeyword));
    }
    parts.push(" ", printIdentifier(node));
    if (node.typeParameterList) {
        // TODO 0 we know we want to call printTypeParameterList, is there some way to do that here?
        parts.push(path.call(print, "typeParameterList"));
    }

    if (node.baseList) {
        parts.push(path.call(print, "baseList"));
    }

    const hasMembers = node.members.length > 0;
    if (hasMembers) {
        parts.push(concat([hardline, "{"]));
        let lineSeparator = hardline;
        if (node.nodeType === "EnumDeclaration") {
            lineSeparator = concat([",", hardline]);
        }
        parts.push(indent(concat([hardline, join(lineSeparator, path.map(print, "members"))])));
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", " ", "}");
    }

    return concat(parts);
};
