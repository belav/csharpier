import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ParameterNode extends SyntaxTreeNode<"Parameter">, HasIdentifier {

}

export const print: PrintMethod<ParameterNode> = (path, options, print) => {
    return printIdentifier(path.getValue());
};
