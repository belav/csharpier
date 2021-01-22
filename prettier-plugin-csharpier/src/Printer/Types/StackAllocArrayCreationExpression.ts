import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"StackAllocArrayCreationExpression"> {
    stackAllocKeyword: SyntaxToken;
    type: SyntaxTreeNode;
}

export const printStackAllocArrayCreationExpression: PrintMethod<StackAllocArrayCreationExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "stackAllocKeyword"), " ", path.call(print, "type")]);
};
