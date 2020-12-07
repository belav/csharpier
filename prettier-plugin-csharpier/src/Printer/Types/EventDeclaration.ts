import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EventDeclarationNode extends SyntaxTreeNode<"EventDeclaration"> {

}

export const print: PrintMethod<EventDeclarationNode> = (path, options, print) => {
    return "TODO EventDeclaration";
};
