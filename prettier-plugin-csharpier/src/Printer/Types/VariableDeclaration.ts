import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface VariableDeclarationNode extends Node<"VariableDeclaration"> {
    type: Node;
}

export const print: PrintMethod<VariableDeclarationNode> = (path, options, print) => {
    const node = path.getValue();

    const parts: Doc[] = [];
    parts.push(path.call(print, "type"));

    const todo = node as any;

    parts.push(concat([" ", getValue(todo.variables[0].identifier)]));

    return concat(parts);
};
