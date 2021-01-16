import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"StackAllocArrayCreationExpression"> {
    stackAllocKeyword: HasValue;
    type: SyntaxTreeNode;
}

export const print: PrintMethod<StackAllocArrayCreationExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "stackAllocKeyword"), " ", path.call(print, "type")]);
};
