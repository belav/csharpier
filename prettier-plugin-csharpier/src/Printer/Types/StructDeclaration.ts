import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StructDeclarationNode extends SyntaxTreeNode<"StructDeclaration"> {}

export const print: PrintMethod<StructDeclarationNode> = (path, options, print) => {
    return "TODO StructDeclaration";
};
