import { Doc } from "prettier";
import { Node } from "../Node";
import { PrintMethod } from "../PrintMethod";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ClassDeclarationNode extends Node<"ClassDeclaration"> {
    modifiers: { value: string, text: string }[];
    identifier: {
        text: string;
    },
    members: Node[],
}

export const print: PrintMethod<ClassDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    node.modifiers.forEach(o => parts.push(o.text));
    parts.push("class");
    parts.push(node.identifier.text);

    const hasMembers = node.members.length > 0;
    const braces: Doc[] = [];
    if (hasMembers) {

    } else {
        braces.push(" ", "{", "}")
    }

    return concat([join(" ", parts), ...braces, hardline]);
};
