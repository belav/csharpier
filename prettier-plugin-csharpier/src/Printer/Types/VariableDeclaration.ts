import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printVariableDeclarator, VariableDeclaratorNode } from "./VariableDeclarator";

export interface VariableDeclarationNode extends SyntaxTreeNode<"VariableDeclaration"> {
    type?: SyntaxTreeNode;
    variables: VariableDeclaratorNode[];
}

export const printVariableDeclaration: PrintMethod<VariableDeclarationNode> = (path, options, print) => {
    return concat([path.call(print, "type"), " ", join(", ", path.map(o => printVariableDeclarator(o, options, print), "variables"))]);
};
