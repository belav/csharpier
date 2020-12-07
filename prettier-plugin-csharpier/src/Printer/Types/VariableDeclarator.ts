import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface VariableDeclaratorNode extends SyntaxTreeNode<"VariableDeclarator">, HasIdentifier {
    initializer?: SyntaxTreeNode;
}

export const print: PrintMethod<VariableDeclaratorNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printIdentifier(node));
    if (node.initializer) {
        parts.push(" ", path.call(print, "initializer"));
    }

    return concat(parts);
};
