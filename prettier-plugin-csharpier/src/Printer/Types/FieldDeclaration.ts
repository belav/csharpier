import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, HasModifiers, HasValue, Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FieldDeclarationNode extends Node<"FieldDeclaration">, HasModifiers {
    declaration: Node;
}

export const print: PrintMethod<FieldDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(join(" ", node.modifiers.map(o => getValue(o))));
    parts.push(" ");

    parts.push(path.call(print, "declaration"));

    parts.push(concat([";"]));

    return concat(parts);
};
