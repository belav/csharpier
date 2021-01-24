import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeConstraintNode extends SyntaxTreeNode<"TypeConstraint"> {
    type?: SyntaxTreeNode;
}

export const printTypeConstraint: PrintMethod<TypeConstraintNode> = (path, options, print) => {
    return path.call(print, "type");
};
