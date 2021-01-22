import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printPathIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SingleVariableDesignationNode extends SyntaxTreeNode<"SingleVariableDesignation">, HasIdentifier {}

export const printSingleVariableDesignation: PrintMethod<SingleVariableDesignationNode> = (path, options, print) => {
    return printPathIdentifier(path);
};
