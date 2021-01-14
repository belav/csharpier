import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ModuloAssignmentExpressionNode extends SyntaxTreeNode<"ModuloAssignmentExpression"> {}

export const print: PrintMethod<ModuloAssignmentExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ModuloAssignmentExpression" : "";
};
