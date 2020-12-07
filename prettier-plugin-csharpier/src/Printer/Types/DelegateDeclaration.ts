import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DelegateDeclarationNode extends SyntaxTreeNode<"DelegateDeclaration"> {}

export const print: PrintMethod<DelegateDeclarationNode> = (path, options, print) => {
    return "TODO DelegateDeclaration";
};
