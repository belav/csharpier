import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeOfExpressionNode extends SyntaxTreeNode<"TypeOfExpression"> {}

export const print: PrintMethod<TypeOfExpressionNode> = (path, options, print) => {
    return "TODO TypeOfExpression";
};
