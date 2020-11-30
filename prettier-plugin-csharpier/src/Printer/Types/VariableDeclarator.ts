import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface VariableDeclaratorNode extends SyntaxTreeNode<"VariableDeclarator">, HasIdentifier {
}

export const print: PrintMethod<VariableDeclaratorNode> = (path, options, print) => {
    return printIdentifier(path.getValue());
};
