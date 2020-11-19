import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, HasModifiers, HasValue, Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface MethodDeclarationNode extends Node<"MethodDeclaration">, HasModifiers {
    returnType: Node;
    identifier: HasValue;
    parameterList: Node;
    body: Node;
}

export const print: PrintMethod<MethodDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push(path.call(print, "returnType"));
    parts.push(" ", getValue(node.identifier));

    parts.push("() {}");

    return concat(parts);
};
