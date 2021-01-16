import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitStackAllocArrayCreationExpressionNode
    extends SyntaxTreeNode<"ImplicitStackAllocArrayCreationExpression"> {
    stackAllocKeyword: HasValue;
    initializer: SyntaxTreeNode;
}

export const print: PrintMethod<ImplicitStackAllocArrayCreationExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "stackAllocKeyword"), "[] ", path.call(print, "initializer")])
};
