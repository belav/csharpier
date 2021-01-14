import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ExclusiveOrAssignmentExpressionNode extends SyntaxTreeNode<"ExclusiveOrAssignmentExpression"> {}

export const print: PrintMethod<ExclusiveOrAssignmentExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ExclusiveOrAssignmentExpression" : "";
};
