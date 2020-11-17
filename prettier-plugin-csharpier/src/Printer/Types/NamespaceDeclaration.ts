import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NamespaceDeclarationNode extends Node<"NamespaceDeclaration"> {
    namespaceKeyword: {
        text: string;
    }
    name: {
        identifier: {
            text: string;
        }
    }
    members: Node[]
}

export const print: PrintMethod<NamespaceDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(node.namespaceKeyword.text);
    parts.push(" ");
    parts.push(node.name.identifier.text);

    const hasMembers = node.members.length > 0;
    const braces: Doc[] = [];
    if (hasMembers) {

    } else {
        braces.push(" ", "{", "}")
    }

    return concat([...parts, ...braces, hardline]);
};
