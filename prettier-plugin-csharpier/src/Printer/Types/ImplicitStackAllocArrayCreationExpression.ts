import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitStackAllocArrayCreationExpressionNode
    extends SyntaxTreeNode<"ImplicitStackAllocArrayCreationExpression"> {
    stackAllocKeyword: SyntaxToken;
    initializer: SyntaxTreeNode;
}

export const printImplicitStackAllocArrayCreationExpression: PrintMethod<ImplicitStackAllocArrayCreationExpressionNode> = (
    path,
    options,
    print
) => {
    return concat([printPathSyntaxToken(path, "stackAllocKeyword"), "[] ", path.call(print, "initializer")]);
};
