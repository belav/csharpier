import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NamespaceDeclarationNode extends Node<"NamespaceDeclaration"> {
    namespaceKeyword: {
        text: string;
    };
    members: Node[];
    usings: Node[];
}

export const print: PrintMethod<NamespaceDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(node.namespaceKeyword.text);
    parts.push(" ");
    parts.push(path.call(print, "name"));

    const hasMembers = node.members.length > 0;
    const hasUsing = node.usings.length > 0;
    if (hasMembers || hasUsing) {
        parts.push(concat([hardline, "{"]));
        parts.push(
            indent(
                concat([
                    hardline,
                    hasUsing ? concat(path.map(print, "usings")) : "",
                    hasMembers ? concat(path.map(print, "members")) : "",
                ]),
            ),
        );
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", "}");
    }

    return concat([concat(parts)]);
};
