import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printPathIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GenericNameNode extends SyntaxTreeNode<"GenericName">, HasIdentifier {}

export const printGenericName: PrintMethod<GenericNameNode> = (path, options, print) => {
    return group(concat([printPathIdentifier(path), "<", path.call(print, "typeArgumentList"), ">"]));
};
