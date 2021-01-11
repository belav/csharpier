import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"StackAllocArrayCreationExpression"> {

}

export const print: PrintMethod<StackAllocArrayCreationExpressionNode> = (path, options, print) => {
    return "TODO StackAllocArrayCreationExpression";
};
