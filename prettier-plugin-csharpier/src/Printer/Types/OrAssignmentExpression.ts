import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface OrAssignmentExpressionNode extends SyntaxTreeNode<"OrAssignmentExpression"> {}

export const print: PrintMethod<OrAssignmentExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node OrAssignmentExpression" : "";
};
