import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface OperatorDeclarationNode extends SyntaxTreeNode<"OperatorDeclaration"> {

}

export const print: PrintMethod<OperatorDeclarationNode> = (path, options, print) => {
    return "TODO OperatorDeclaration";
};
