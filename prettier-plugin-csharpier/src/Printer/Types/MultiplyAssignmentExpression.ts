import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface MultiplyAssignmentExpressionNode extends SyntaxTreeNode<"MultiplyAssignmentExpression"> {}

export const print: PrintMethod<MultiplyAssignmentExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node MultiplyAssignmentExpression" : "";
};
