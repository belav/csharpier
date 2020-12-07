import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printModifiers } from "../PrintModifiers";
import { HasIdentifier, HasModifiers, printIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConstructorDeclarationNode
    extends SyntaxTreeNode<"ConstructorDeclaration">,
        HasModifiers,
        HasIdentifier {}

export const print: PrintMethod<ConstructorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push(printIdentifier(node));
    parts.push("()");
    parts.push(path.call(print, "body"));

    return group(concat(parts));
};
