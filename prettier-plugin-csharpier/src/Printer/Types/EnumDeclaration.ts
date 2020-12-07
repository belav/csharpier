import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EnumDeclarationNode extends SyntaxTreeNode<"EnumDeclaration"> {

}

export const print: PrintMethod<EnumDeclarationNode> = (path, options, print) => {
    return "TODO EnumDeclaration";
};
