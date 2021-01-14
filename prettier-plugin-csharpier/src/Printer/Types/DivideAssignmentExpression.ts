import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DivideAssignmentExpressionNode extends SyntaxTreeNode<"DivideAssignmentExpression"> {}

export const print: PrintMethod<DivideAssignmentExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node DivideAssignmentExpression" : "";
};
