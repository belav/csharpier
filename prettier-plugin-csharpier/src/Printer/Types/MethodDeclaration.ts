import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, HasModifiers, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface MethodDeclarationNode extends SyntaxTreeNode<"MethodDeclaration">, HasModifiers {
    returnType: SyntaxTreeNode;
    identifier: HasValue;
    parameterList: SyntaxTreeNode;
    body: SyntaxTreeNode;
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
