import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConstructorDeclarationNode extends SyntaxTreeNode<"ConstructorDeclaration"> {

}

export const print: PrintMethod<ConstructorDeclarationNode> = (path, options, print) => {
    return "TODO ConstructorDeclaration";
};
