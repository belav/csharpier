import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ParameterNode extends SyntaxTreeNode<"Parameter">, HasIdentifier {
    type?: SyntaxTreeNode;
}

export const print: PrintMethod<ParameterNode> = (path, options, print) => {
    const node = path.getValue();
    const identifier = printIdentifier(path.getValue());

    if (node.type) {
        return concat([path.call(print, "type"), " ", identifier]);
    }

    return identifier;
};
