import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolationNode extends SyntaxTreeNode<"Interpolation"> {
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<InterpolationNode> = (path, options, print) => {
    return concat(["{", path.call(print, "expression"), "}"]);
};
