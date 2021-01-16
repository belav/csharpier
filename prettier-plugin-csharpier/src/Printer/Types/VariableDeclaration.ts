import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface VariableDeclarationNode extends SyntaxTreeNode<"VariableDeclaration"> {
    type: SyntaxTreeNode;
}

// TODO force these onto new lines?? but this gets used inside of things too
export const print: PrintMethod<VariableDeclarationNode> = (path, options, print) => {
    return concat([path.call(print, "type"), " ", join(", ", path.map(print, "variables"))]);
};
