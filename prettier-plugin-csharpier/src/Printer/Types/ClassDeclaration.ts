import { Doc } from "prettier";
import { getValue, HasModifiers, HasValue, Node } from "../Node";
import { PrintMethod } from "../PrintMethod";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface ClassDeclarationNode extends Node<"ClassDeclaration">, HasModifiers {
    identifier: HasValue;
    members: Node[];
}

export const print: PrintMethod<ClassDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push("class");
    parts.push(" ", getValue(node.identifier));

    const hasMembers = node.members.length > 0;
    if (hasMembers) {
        parts.push(concat([hardline, "{"]));
        parts.push(indent(concat([hardline, join(hardline, path.map(print, "members"))])));
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", " ", "}");
    }

    return concat(parts);
};
