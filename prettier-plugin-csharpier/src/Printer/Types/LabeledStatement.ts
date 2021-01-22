import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printPathIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LabeledStatementNode extends SyntaxTreeNode<"LabeledStatement">, HasIdentifier {}

export const printLabeledStatement: PrintMethod<LabeledStatementNode> = (path, options, print) => {
    return concat([printPathIdentifier(path), ":", hardline, path.call(print, "statement")]);
};
