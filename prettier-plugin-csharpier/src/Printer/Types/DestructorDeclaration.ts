import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printModifiers } from "../PrintModifiers";
import { HasIdentifier, HasValue, printIdentifier, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DestructorDeclarationNode extends SyntaxTreeNode<"DestructorDeclaration">, HasIdentifier {
    tildeToken: HasValue;
    parameterList: SyntaxTreeNode;
    body: SyntaxTreeNode;
}

export const print: PrintMethod<DestructorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printValue(node.tildeToken));
    parts.push(printIdentifier(node));
    parts.push(path.call(print, "parameterList"));
    parts.push(path.call(print, "body"));

    return group(concat(parts));
};
