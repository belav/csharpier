import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitStackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitStackAllocArrayCreationExpression"> {

}

export const print: PrintMethod<ImplicitStackAllocArrayCreationExpressionNode> = (path, options, print) => {
    return "TODO ImplicitStackAllocArrayCreationExpression";
};
