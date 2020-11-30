import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterfaceDeclarationNode extends SyntaxTreeNode<"InterfaceDeclaration"> {

}

export const print: PrintMethod<InterfaceDeclarationNode> = (path, options, print) => {
    return "TODO InterfaceDeclaration";
};
