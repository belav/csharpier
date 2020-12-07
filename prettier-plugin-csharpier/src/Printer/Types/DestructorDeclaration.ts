import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DestructorDeclarationNode extends SyntaxTreeNode<"DestructorDeclaration"> {}

export const print: PrintMethod<DestructorDeclarationNode> = (path, options, print) => {
    return "TODO DestructorDeclaration";
};
