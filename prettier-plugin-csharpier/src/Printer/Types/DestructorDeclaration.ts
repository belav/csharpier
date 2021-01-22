import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printIdentifier, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DestructorDeclarationNode extends SyntaxTreeNode<"DestructorDeclaration">, HasIdentifier {
    tildeToken: SyntaxToken;
    parameterList: SyntaxTreeNode;
    body: SyntaxTreeNode;
}

export const printDestructorDeclaration: PrintMethod<DestructorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printSyntaxToken(node.tildeToken));
    parts.push(printIdentifier(node));
    parts.push(path.call(print, "parameterList"));
    parts.push(path.call(print, "body"));

    return group(concat(parts));
};
