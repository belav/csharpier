import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArgumentNode extends SyntaxTreeNode<"Argument"> {
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<ArgumentNode> = (path, options, print) => {
    return path.call(print, "expression");
};
