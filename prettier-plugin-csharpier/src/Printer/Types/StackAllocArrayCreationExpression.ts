import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { InitializerExpressionNode, printInitializerExpression } from "./InitializerExpression";

export interface StackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"StackAllocArrayCreationExpression"> {
    stackAllocKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    initializer?: InitializerExpressionNode;
}

export const printStackAllocArrayCreationExpression: PrintMethod<StackAllocArrayCreationExpressionNode> = (
    path,
    options,
    print
) => {
    const node = path.getValue();
    return concat([
        "stackalloc ",
        path.call(print, "type"),
        node.initializer
            ? concat([" ", path.call(o => printInitializerExpression(o, options, print), "initializer")])
            : ""
    ]);
};
