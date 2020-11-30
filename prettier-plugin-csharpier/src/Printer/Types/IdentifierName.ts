import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode, HasIdentifier, printIdentifier } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IdentifierNameNode extends SyntaxTreeNode<"IdentifierName">, HasIdentifier{
}

export const print: PrintMethod<IdentifierNameNode> = (path, options, print) => {
    return printIdentifier(path.getValue());
};
