import { PrintMethod } from "../PrintMethod";
import {
    printSyntaxToken,
    SyntaxToken,
    SyntaxTreeNode,
    HasIdentifier,
    printIdentifier,
    printPathSyntaxToken,
    printPathIdentifier
} from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IdentifierNameNode extends SyntaxTreeNode<"IdentifierName">, HasIdentifier {}

export const printIdentifierName: PrintMethod<IdentifierNameNode> = (path, options, print) => {
    return printPathIdentifier(path);
};
