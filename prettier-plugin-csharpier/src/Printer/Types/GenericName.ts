import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GenericNameNode extends SyntaxTreeNode<"GenericName">, HasIdentifier {

}

export const print: PrintMethod<GenericNameNode> = (path, options, print) => {
    return group(concat([printIdentifier(path.getValue()), "<", path.call(print, "typeArgumentList"), ">"]));
};
