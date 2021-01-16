import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConstantPatternNode extends SyntaxTreeNode<"ConstantPattern"> {
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<ConstantPatternNode> = (path, options, print) => {
    return path.call(print, "expression");
};
