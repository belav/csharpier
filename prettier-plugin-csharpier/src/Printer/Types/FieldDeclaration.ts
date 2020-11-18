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

    const todo = node.declaration as any;

    if (todo.type.nodeType == "PredefinedType") {
        parts.push(getValue(todo.type.keyword));
    } else if (todo.type.nodeType == "IdentifierName") {
        parts.push(path.call(print, "declaration", "type"));
    }

    parts.push(concat([" ", getValue(todo.variables[0].identifier)]));

    parts.push(concat([";"]));

    return concat(parts);
};
