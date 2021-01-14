import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SubtractExpressionNode extends SyntaxTreeNode<"SubtractExpression"> {}

export const print: PrintMethod<SubtractExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node SubtractExpression" : "";
};
