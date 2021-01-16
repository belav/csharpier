import { Doc } from "prettier";
import { concat, hardline, indent, join } from "./Builders";
import { printComments } from "./Comments";
import { PrintMethod } from "./PrintMethod";
import {
    HasIdentifier,
    HasModifiers,
    HasValue,
    printIdentifier,
    printModifiers,
    printValue,
    SyntaxTreeNode
} from "./SyntaxTreeNode";
import { BaseListNode } from "./Types/BaseList";

export interface ClassLikeDeclarationNode extends SyntaxTreeNode<"ClassDeclaration" | "EnumDeclaration" | "StructDeclaration">, HasModifiers, HasIdentifier {
    keyword?: HasValue;
    enumKeyword?: HasValue;
    members: SyntaxTreeNode[];
    baseList: BaseListNode;
}

export const print: PrintMethod<ClassLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printComments(parts, node);
    parts.push(printModifiers(node));
    if (node.keyword) {
        parts.push(printValue(node.keyword));
    }
    if (node.enumKeyword) {
        parts.push(printValue(node.enumKeyword));
    }
    parts.push(" ", printIdentifier(node));

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
