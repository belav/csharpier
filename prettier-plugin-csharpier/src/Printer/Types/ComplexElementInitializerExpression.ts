import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ComplexElementInitializerExpressionNode
    extends SyntaxTreeNode<"ComplexElementInitializerExpression"> {}

export const print: PrintMethod<ComplexElementInitializerExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ComplexElementInitializerExpression" : "";
};
