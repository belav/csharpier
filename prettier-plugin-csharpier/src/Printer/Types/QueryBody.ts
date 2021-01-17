import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface QueryBodyNode extends SyntaxTreeNode<"QueryBody"> {
    clauses: SyntaxTreeNode[];
    selectOrGroup: SyntaxTreeNode;
    continuation?: SyntaxTreeNode;
}

export const print: PrintMethod<QueryBodyNode> = (path, options, print) => {
    return path.call(print, "selectOrGroup");
};
