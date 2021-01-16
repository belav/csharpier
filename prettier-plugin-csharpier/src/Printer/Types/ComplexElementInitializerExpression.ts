import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ComplexElementInitializerExpressionNode
    extends SyntaxTreeNode<"ComplexElementInitializerExpression"> {
    expressions: SyntaxTreeNode[]
}

export const print: PrintMethod<ComplexElementInitializerExpressionNode> = (path, options, print) => {
    return concat(["{", " ", join(", ", path.map(print, "expressions")), " ", "}"]);
};
