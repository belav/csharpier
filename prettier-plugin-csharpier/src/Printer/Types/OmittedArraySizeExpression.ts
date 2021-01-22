import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface OmittedArraySizeExpressionNode extends SyntaxTreeNode<"OmittedArraySizeExpression"> {}

export const printOmittedArraySizeExpression: PrintMethod<OmittedArraySizeExpressionNode> = (path, options, print) => {
    return "";
};
