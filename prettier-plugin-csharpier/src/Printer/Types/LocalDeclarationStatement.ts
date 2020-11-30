import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LocalDeclarationStatementNode extends SyntaxTreeNode<"LocalDeclarationStatement"> {

}

export const print: PrintMethod<LocalDeclarationStatementNode> = (path, options, print) => {
    return "TODO LocalDeclarationStatement";
};
